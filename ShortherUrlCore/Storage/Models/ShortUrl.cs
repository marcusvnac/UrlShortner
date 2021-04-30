using Microsoft.Azure.Cosmos.Table;

namespace ShortherUrlCore.Storage.Models
{
    public class ShortUrl : TableEntity
    {
        public ShortUrl()
        {

        }

        public ShortUrl(string hashUrl, string originalUrl)
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
