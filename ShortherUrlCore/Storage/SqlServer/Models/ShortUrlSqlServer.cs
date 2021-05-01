using Dapper.Contrib.Extensions;

namespace ShortherUrlCore.Storage.SqlServer.Models
{
    [Table("ShortUrl")]
    public class ShortUrlSqlServer
    {
        [ExplicitKey]
        public string ShortnedUrl { get; set; }
        public string OriginalUrl { get; set; }
    }
}
