using System.Data;

namespace Products.Api.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection BuildConnection();
    }
    public class DbConnectionFactory: IDbConnectionFactory
    {
        public IDbConnection BuildConnection()
        {
            throw new System.NotImplementedException();
        }
    }
}