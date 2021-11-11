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
        Task<Models.ProductOptions> GetOptionsAsync(Guid Id, string trackId);
        Task<Models.ProductOption> GetOptionAsync(Guid productId, Guid id, string trackId);

        Task AddAsync(Product product, string trackId);
        Task AddAsync(Guid productId, ProductOption option, string trackId);

        Task UpdateAsync(Guid id, Product product, string trackId);
        Task UpdateAsync(Guid productId, Guid id, ProductOption option, string trackId);

        Task DeleteAsync(Guid id, string trackId);
        Task DeleteAsync(Guid productId, Guid id, string trackId);
    }

    public class ProductRepository : IProductRepository
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
                var products = await conn.QueryAsync<Product>(sql);

                return new Models.Products(products);
            }
        }

        public Task<Models.Product> GetAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductOptions> GetOptionsAsync(Guid Id, string trackId)
        {
            /*Items = new List<ProductOption>();
                        var conn = Helpers.NewConnection();
                        conn.Open();
                        var cmd = conn.CreateCommand();

                        cmd.CommandText = $"select id from productoptions {where}";

                        var rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            var id = Guid.Parse(rdr.GetString(0));
                            Items.Add(new ProductOption(id));
                        }*/
            throw new NotImplementedException();
        }

        public Task<ProductOption> GetOptionAsync(Guid productId, Guid id, string trackId)
        {
            /*var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"select * from productoptions where id = '{id}' collate nocase";

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return;

            Id = Guid.Parse(rdr["Id"].ToString());
            ProductId = Guid.Parse(rdr["ProductId"].ToString());
            Name = rdr["Name"].ToString();
            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();*/

            throw new NotImplementedException();
        }

        public Task AddAsync(Product product, string trackId)
        { /*
           var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = IsNew
                ? $"insert into Products (id, name, description, price, deliveryprice) values ('{Id}', '{Name}', '{Description}', {Price}, {DeliveryPrice})"
                : $"update Products set name = '{Name}', description = '{Description}', price = {Price}, deliveryprice = {DeliveryPrice} where id = '{Id}' collate nocase";

            conn.Open();
            cmd.ExecuteNonQuery();
           */
            throw new NotImplementedException();
        }

        public Task AddAsync(Guid productId, ProductOption option, string trackId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, Product product, string trackId)
        {
            /*var orig = new Product(id)
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };

            if (!orig.IsNew)
                orig.Save();

            await Task.FromResult(true);

            return new OkResult();*/

            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid productId, Guid id, ProductOption option, string trackId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, string trackId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid productId, Guid id, string trackId)
        {
            throw new NotImplementedException();
        }
    }
}