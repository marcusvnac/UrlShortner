using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using ShortherUrlCore.Models;
using ShortherUrlCore.Storage.SqlServer.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ShortherUrlCore.Storage.SqlServer
{
    public class SqlServerStorageManager : IStorageManager
    {
        private readonly string connectionString;
        private readonly IDbConnection DbConnection;

        public SqlServerStorageManager(IConfiguration Configuration)
        {
            this.connectionString = Configuration.GetConnectionString("SqlServer");
            this.DbConnection = GetDbConnection();
        }

        public Task<ShortUrl> Get(string hashUrl)
        {
            ShortUrl res = null;

            var shortUrlData = DbConnection.Get<ShortUrlSqlServer>(hashUrl);
            if (shortUrlData != null)
            {
                res = new ShortUrl { OriginalUrl = shortUrlData.OriginalUrl, ShortnedUrl = shortUrlData.ShortnedUrl };
            }

            return Task.FromResult(res);
        }

        public Task Insert(ShortUrl shortUrl)
        {
            DbConnection.Insert(new ShortUrlSqlServer { ShortnedUrl = shortUrl.ShortnedUrl, OriginalUrl = shortUrl.OriginalUrl });

            return Task.CompletedTask;
        }

        private IDbConnection GetDbConnection()
        {
            var dbConnection = new SqlConnection(this.connectionString);
            dbConnection.Open();
            return dbConnection;
        }
    }
}
