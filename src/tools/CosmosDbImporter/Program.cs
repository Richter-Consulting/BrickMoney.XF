using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using CosmosDbImporter.Helpers;
using BrickMoney;

namespace CosmosDbImporter
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var importer = new CosmosImporter();
            string filePath = @"../../../data/LegoSets.json";
            await importer.ImportFromFileAsync(filePath);
        }
    }

    public class CosmosImporter
    {
        private static readonly string _endpoint = Secrets.CosmosEndpoint;

        private static readonly string _key = Secrets.CosmosKey;

        private static readonly string _database = "logs";
        private static readonly string _containerName = "BrickBasicInfo";
        private readonly CosmosClient _client;

        public CosmosImporter()
        {
            _client = new CosmosClient(_endpoint, _key);
            Console.WriteLine($"Client created {_client.Endpoint}");
        }

        public async Task ImportFromFileAsync(string filePath)
        {
            if (!File.Exists(filePath)) {
                Console.WriteLine($"{DateTime.Now:O} File doesn't exists: {filePath}");
                return;
            }
            if (string.IsNullOrWhiteSpace(_endpoint)) {
                Console.WriteLine($"{DateTime.Now:O} Endpoint is empty!");
                return;
            }
            if (string.IsNullOrWhiteSpace(_key)) {
                Console.WriteLine($"{DateTime.Now:O} Key is empty!");
                return;
            }
            var db = await CreateDatabaseAsync(_client);
            var container = await CreateContainerAsync(db);
            var dataForImport = await ReadDataFromFile(filePath);
            await ImportAllData(container, dataForImport);
        }

        private async Task<Database> CreateDatabaseAsync(CosmosClient cosmosClient)
        {
            // Create a new database 
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(_database);
            Console.WriteLine($"{DateTime.Now:O} Created Database: {database.Database.Id}");
            return database.Database;
        }

        private async Task<Container> CreateContainerAsync(Database database)
        {
            // Create a new container 
            var container = await database.CreateContainerIfNotExistsAsync(_containerName, "/partitionKey");
            Console.WriteLine($"{DateTime.Now:O} Created Container: {container.Container.Id}");
            return container.Container;
        }

        private async Task<IList<LegoSetBasicInfo>> ReadDataFromFile(string filePath)
        {
            List<LegoSetBasicInfo> legoSets = JsonConvert.DeserializeObject<List<LegoSetBasicInfo>>(await System.IO.File.ReadAllTextAsync(filePath));
            Console.WriteLine($"{DateTime.Now:O} Found {legoSets.Count} LEGO set for import.");
            return legoSets;
        }

        private async Task ImportAllData(Container container, IList<LegoSetBasicInfo> data)
        {
            var partitionKey = new PartitionKey("default");
            var batch = container.CreateTransactionalBatch(partitionKey);
            var count = 0;
            var convertedData = data.Select(s => new LegoSetBasicInfoCosmosDB
            {
                LegoSetId = s.LegoSetId.ToString("0"),
                Year = s.Year, RrPrice = s.RrPrice, NameDE = s.NameDE,
                NameEN = s.NameEN
            });

            foreach (var cosmosData in convertedData)
            {
                count++;
                batch.UpsertItem(cosmosData);
                if (count % 30 == 0)
                {
                    var response = await batch.ExecuteAsync();
                    Console.WriteLine($"{DateTime.Now:O} Response Code: {response.StatusCode}");
                    Console.WriteLine($"{DateTime.Now:O} Response Charge: {response.RequestCharge}");
                    Console.WriteLine($"{DateTime.Now:O} Saved {count} files");
                    batch = container.CreateTransactionalBatch(partitionKey);
                }
            }

            // Final save
            var finalResponse = await batch.ExecuteAsync();
            Console.WriteLine($"{DateTime.Now:O} Response Code: {finalResponse.StatusCode}");
            Console.WriteLine($"{DateTime.Now:O} Response Charge: {finalResponse.RequestCharge}");
            Console.WriteLine($"{DateTime.Now:O} Saved {count} files");
        }
    }
}
