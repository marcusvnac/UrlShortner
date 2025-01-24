using Dapper.Contrib.Extensions;

namespace ShortherUrlCore.Storage.SqlServer.Models
{
    [Table("ShortUrl")]
    public class ShortUrlSqlServer
    {
        [ExplicitKey]
        public required string ShortnedUrl { get; set; }
        public required string OriginalUrl { get; set; }
    }
}
