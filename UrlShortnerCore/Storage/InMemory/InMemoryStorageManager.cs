using ShortherUrlCore.Models;

namespace ShortherUrlCore.Storage.InMemory
{
    public class InMemoryStorageManager : IStorageManager
    {
        private readonly Dictionary<string, string> DataStorage;

        public InMemoryStorageManager()
        {
            this.DataStorage = new Dictionary<string, string>();
        }

        public Task<ShortUrl?> Get(string hashUrl)
        {
            ShortUrl? result = null;
            if (this.DataStorage.TryGetValue(hashUrl, out string? originalUrl))
            {
                result = new ShortUrl { ShortnedUrl = hashUrl, OriginalUrl = originalUrl };
            }

            return Task.FromResult(result);
        }

        public Task Insert(ShortUrl shortUrl)
        {
            if (!this.DataStorage.ContainsKey(shortUrl.ShortnedUrl))
            {
                this.DataStorage.Add(shortUrl.ShortnedUrl, shortUrl.OriginalUrl);
            }
            return Task.CompletedTask;
        }
    }
}
