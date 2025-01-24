using ShortherUrlCore.Models;

namespace ShortherUrlCore.Storage
{
    public interface IStorageManager
    {
        Task<ShortUrl?> Get(string hashUrl);

        Task Insert(ShortUrl shortUrl);
    }
}
