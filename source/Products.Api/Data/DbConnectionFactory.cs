using System.Data;
using Microsoft.Data.Sqlite;
using Products.Api.Models;

namespace Products.Api.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection BuildConnection();
    }
    public class DbConnectionFactory: IDbConnectionFactory
    {
        private readonly IAppSettings _appSettings;

        public DbConnectionFactory(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public IDbConnection BuildConnection()
        {
            var conn =new SqliteConnection(_appSettings.ConnectionString);

            return conn;
        }
    }
}