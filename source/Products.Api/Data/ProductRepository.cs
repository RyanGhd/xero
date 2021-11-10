using System;
using System.Threading.Tasks;
using Dapper;
using Products.Api.Models;

namespace Products.Api.Data
{
    public interface IProductRepository
    {
        Task<Models.Products> GetAsync();
        Task<Models.Product> GetAsync(Guid Id);
    }

    public class ProductRepository: IProductRepository
    {
        private const string ProductTable = "product";

        private readonly IDbConnectionFactory _connectionFactory;

        public ProductRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Models.Products> GetAsync()
        {
            var sql = $@"SELECT * from {ProductTable}";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var products =await conn.QueryAsync<Product>(sql);

                return new Models.Products(products);
            }
        }

        public Task<Models.Product> GetAsync(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}