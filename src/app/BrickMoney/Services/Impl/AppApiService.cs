using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrickMoney.Helpers;
using BrickMoney.Models.Api;
using Microsoft.Azure.Cosmos;
using WD.Logging.Abstractions;

namespace BrickMoney.Services.Impl
{
    public class AppApiService : IApiService
    {
        private readonly ILogger<AppApiService> _logger;
        private readonly IAppSettings _settings;

        public AppApiService(ILogger<AppApiService> logger, IAppSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public async Task<IList<SetBasicInfo>> GetNewSets()
        {
            try
            {
                var endpoint = Secrets.CosmosEndpoint;
                var lastEndpoint = _settings.LastUpdateEndpoint;
                var lastTimestamp = _settings.LastUpdateTimeStamp;
                if (!string.Equals(endpoint, lastEndpoint))
                {
                    lastTimestamp = int.MinValue;
                }

                var key = Secrets.CosmosKey;
                var dbName = Secrets.CosmosDb;
                var basicContainer = Secrets.CosmosBasicContainer;
                var allData = new List<SetBasicInfo>();

                using (var client = new CosmosClient(endpoint, key))
                {
                    var cosmosDb = client.GetDatabase(dbName);
                    var container = cosmosDb.GetContainer(basicContainer);
                    // Max Time stamp lesen!
                    var maxIterator = container.GetItemQueryIterator<MaxTimestamp>("SELECT MAX(c._ts) AS max FROM c");
                    var maxData = await maxIterator.ReadNextAsync();
                    var maxTimeStamp = maxData.First().Max;

                    // Daten abfragen
                    var dataQuery = lastTimestamp < 0
                        ? "SELECT * FROM c"
                        : $"SELECT * FROM c WHERE c._ts > {lastTimestamp}";
                    var iterator = container.GetItemQueryIterator<SetBasicInfo>(dataQuery);

                    while (iterator.HasMoreResults)
                    {
                        allData.AddRange(await iterator.ReadNextAsync());
                    }

                    _settings.LastUpdateTimeStamp = maxTimeStamp;
                    _settings.LastUpdateEndpoint = endpoint;
                }

                return allData;
            } catch(Exception ex)
            {
                _logger.Error(ex, "Failed to get new sets");
                throw;
            }
        }
    }
}
