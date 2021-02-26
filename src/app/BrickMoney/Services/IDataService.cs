using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BrickMoney.Models.Data;

namespace BrickMoney.Services
{
    public interface IDataService : IDisposable
    {
        Task SyncDataAsync();
        Task<IList<SimpleUserSet>> GetLocalDataAsync();

        Task<LegoSetBasicInfo> GetBasicInfoForUserInfo(int userInfoId);
    }
}
