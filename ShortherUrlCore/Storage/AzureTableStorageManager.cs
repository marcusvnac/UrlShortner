using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShortherUrlCore.Storage.Models;
using System;
using System.Threading.Tasks;

namespace ShortherUrlCore.Storage
{
    public class AzureTableStorageManager : IStorageManager
    {
        private readonly string connectionString;
        private readonly ILogger<AzureTableStorageManager> logger;
        private const string TableName = "ShortUrl";

        public AzureTableStorageManager(ILogger<AzureTableStorageManager> logger, IConfiguration Configuration)
        {
            this.connectionString = Configuration.GetConnectionString("AzureTableStorage");
            this.logger = logger;
        }

        public async Task Upsert(ShortUrl shortUrl)
        {
            var tableRef = await CreateTableAsync();

            await InsertOrMergeEntityAsync(tableRef, shortUrl);
        }

        public async Task<ShortUrl> Get(string hashUrl)
        {
            var tableRef = await CreateTableAsync();

            var res = await RetrieveEntityUsingPointQueryAsync(tableRef, hashUrl, hashUrl);

            return res;
        }


        private CloudStorageAccount CreateStorageAccountFromConnectionString()
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(connectionString);
            }
            catch (FormatException)
            {
                logger.LogError("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                logger.LogError("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                throw;
            }

            return storageAccount;
        }

        private async Task<CloudTable> CreateTableAsync()
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString();

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            Console.WriteLine("Create a Table for the demo");

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(TableName);
            if (await table.CreateIfNotExistsAsync())
            {
                logger.LogInformation("Created Table named: {0}", TableName);
            }
            else
            {
                logger.LogInformation("Table {0} already exists", TableName);
            }

            Console.WriteLine();
            return table;
        }

        private async Task<ShortUrl> InsertOrMergeEntityAsync(CloudTable table, ShortUrl entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("ShortUrl");
            }

            try
            {
                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                // Execute the operation.
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                ShortUrl insertedCustomer = result.Result as ShortUrl;

                if (result.RequestCharge.HasValue)
                {
                    logger.LogTrace("Request Charge of InsertOrMerge Operation: " + result.RequestCharge);
                }

                return insertedCustomer;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        private async Task<ShortUrl> RetrieveEntityUsingPointQueryAsync(CloudTable table, string partitionKey, string rowKey)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<ShortUrl>(partitionKey, rowKey);
                TableResult result = await table.ExecuteAsync(retrieveOperation);
                ShortUrl shortUrl = result.Result as ShortUrl;
                if (shortUrl != null)
                {
                    logger.LogDebug("\t{0}\t{1}\t{2}\t{3}", shortUrl.PartitionKey, shortUrl.RowKey, shortUrl.HashUrl, shortUrl.OriginalUrl);
                }

                if (result.RequestCharge.HasValue)
                {
                    logger.LogDebug("Request Charge of Retrieve Operation: " + result.RequestCharge);
                }

                return shortUrl;
            }
            catch (StorageException e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
