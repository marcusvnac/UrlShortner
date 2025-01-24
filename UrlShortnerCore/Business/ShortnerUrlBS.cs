using ShortherUrlCore.Models;
using ShortherUrlCore.Storage;
using System.Security.Cryptography;
using System.Text;

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
            var hashCode = CalculateHash(originalUrl);

            return await GetHashUrl(hashCode, originalUrl);
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

            await storageManager.Insert(new ShortUrl { OriginalUrl = originalUrl, ShortnedUrl = hashUrl });

            return hashUrl;
        }

        /// <summary>
        /// Knuth hash
        /// </summary>
        private string CalculateHash(string text)
        {
            using (var md5Hasher = MD5.Create())
            {
                var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(text));
                return BitConverter.ToString(data).Replace("-", "").Substring(0, 10);
            }
        }
    }
}
