using ShortherUrlCore.Storage;
using ShortherUrlCore.Storage.Models;
using System;
using System.Threading.Tasks;

namespace ShortherUrlCore.Business
{
    public class ShortnerUrlBS : IShortnerUrlBS
    {
        private readonly IStorageManager storageManager;

        public ShortnerUrlBS(IStorageManager storageManager)
        {
            this.storageManager = storageManager;
        }
        
        public async Task<string> Process(string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                throw new ArgumentException(nameof(originalUrl));
            }

            return await Shortner(originalUrl);
        }

        private async Task<string> Shortner(string originalUrl)
        {
            var hashCode =  originalUrl.GetHashCode(StringComparison.InvariantCultureIgnoreCase);

            var hashUrl = hashCode.ToString("X8");

            return await GetHashUrl(hashUrl, originalUrl);
        }

        // Checks if the new Hash has collision. Change it in case and returns a valid unique value
        private async Task<string> GetHashUrl(string hashUrl, string originalUrl)
        {
            int counter = 0;
            ShortUrl storedShortUrl;
            do
            {
                storedShortUrl = await storageManager.Get(hashUrl);
                if (storedShortUrl != null)
                {
                    if (storedShortUrl.OriginalUrl.Equals(originalUrl, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return hashUrl;
                    }
                    hashUrl += Convert.ToChar(counter);
                }

            } while (storedShortUrl != null);

            await storageManager.Upsert(new ShortUrl (hashUrl, originalUrl));

            return hashUrl;
        }
    }
}
