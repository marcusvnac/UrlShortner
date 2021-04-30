using System.Threading.Tasks;

namespace ShortherUrlCore.Business
{
    public interface IShortnerUrlBS
    {
        Task<string> Process(string originalUrl);
    }
}
