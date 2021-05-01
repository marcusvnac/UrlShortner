using ShortherUrlCore.Models;
using System.Threading.Tasks;

namespace ShortherUrlCore.Storage
{
    public interface IStorageManager
    {
        Task<ShortUrl> Get(string hashUrl);

        Task Upsert(ShortUrl shortUrl);
    }
}
