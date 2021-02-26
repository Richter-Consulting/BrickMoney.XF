using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrickMoney.DB;
using BrickMoney.DB.Models;
using BrickMoney.Events;
using BrickMoney.Models.Data;
using Microsoft.EntityFrameworkCore;
using Prism.Events;
using WD.Logging.Abstractions;

namespace BrickMoney.Services.Impl
{
    public sealed class AppDataService : IDataService
    {
        private readonly ILogger<AppDataService> _logger;
        private readonly BrickMoneyDB _dbContext;
        private readonly IApiService _api;
        private readonly IEventAggregator _eventAggregator;

        public AppDataService(ILogger<AppDataService> logger, BrickMoneyDB dbContext, IApiService api, IEventAggregator eventAggregator)
        {
            _logger = logger;
            _dbContext = dbContext;
            _api = api;
            _eventAggregator = eventAggregator;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public Task<LegoSetBasicInfo> GetBasicInfoForUserInfo(int userInfoId)
        {
            return _dbContext.UserInfo
                .AsNoTracking()
                .Include(i => i.LegoSetBasic)
                .Where(w => w.Id == userInfoId)
                .Select(s => s.LegoSetBasic)
                .SingleOrDefaultAsync();
        }

        public async Task<IList<SimpleUserSet>> GetLocalDataAsync()
        {
            return await _dbContext.UserInfo
                .AsNoTracking()
                .OrderByDescending(o => o.PurchaseDate)
                .Select(s => new SimpleUserSet { Id = s.Id, Name = s.LegoSetBasic.NameDE })
                .ToArrayAsync();
        }

        public async Task SyncDataAsync()
        {
            try
            {
                var newCosmosData = await _api.GetNewSets();
                var existingIds = _dbContext.BasicInfo.AsNoTracking().Select(s => s.LegoSetId).ToArray();

                foreach(var newData in newCosmosData)
                {
                    var dbData = new LegoSetBasicInfo
                    {
                        LegoSetId = int.Parse(newData.SetId),
                        NameDE = newData.NameDE,
                        NameEN = newData.NameEN,
                        RrPrice = newData.RrPrice,
                        Year = newData.Year
                    };

                    if (existingIds.Contains(dbData.LegoSetId))
                    {
                        _dbContext.BasicInfo.Update(dbData);
                    } else
                    {
                        _dbContext.BasicInfo.Add(dbData);
                    }
                }

                await _dbContext.SaveChangesAsync();

                if (await _dbContext.UserInfo.CountAsync() == 0)
                {
                    _dbContext.UserInfo.AddRange(new[] {
                        new LegoSetUserInfo{Buyer = "Me", LegoSetId = 2233, SoldOver = "Amazon"},
                        new LegoSetUserInfo{Buyer = "You", LegoSetId = 5864, SoldOver = "eBay"}
                    });
                }

                await _dbContext.SaveChangesAsync();

                // Cache leeren
                foreach(var entity in _dbContext.ChangeTracker.Entries<LegoSetBasicInfo>().ToArray())
                {
                    entity.State = EntityState.Detached;
                }

                // Benachrichtigung, dass Sync abgeschlossen ist
                _eventAggregator.GetEvent<SyncFinished>().Publish(newCosmosData.Any());
            }
            catch (Exception ex)
            {
                _logger.Error("Failed on sync", ex);
                throw;
            }
        }
    }
}
