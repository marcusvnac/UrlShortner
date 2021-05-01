using Microsoft.Azure.Cosmos.Table;

namespace ShortherUrlCore.Storage.Models.AzureTableStorage
{
    public class ShortUrlTableStorage : TableEntity
    {
        public ShortUrlTableStorage()
        {

        }

        public ShortUrlTableStorage(string hashUrl, string originalUrl)
        {
            PartitionKey = hashUrl;
            RowKey = hashUrl;

            this.HashUrl = hashUrl;
            this.OriginalUrl = originalUrl;
        }

        public string HashUrl { get; set; }
        public string OriginalUrl { get; set; }
    }
}
