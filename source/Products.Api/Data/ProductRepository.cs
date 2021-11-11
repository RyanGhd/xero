using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Products.Api.Data.Entities;
using Products.Api.Data.Mappers;
using Products.Api.Models;
using Products.Api.Models.Exceptions;

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
        private const string ProductTable = "Products";
        private const string ProductOptionTable = "ProductOptions";

        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IProductToProductEntityMapper _productMapper;
        private readonly IProductOptionToProductOptionEntityMapper _optionMapper;

        public ProductRepository(
            IDbConnectionFactory connectionFactory,
            IProductToProductEntityMapper productMapper,
            IProductOptionToProductOptionEntityMapper optionMapper)
        {
            _connectionFactory = connectionFactory;
            _productMapper = productMapper;
            _optionMapper = optionMapper;
        }

        public async Task<Models.Products> GetAsync()
        {
            var sql = $@"SELECT * from {ProductTable}";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var entities = await conn.QueryAsync<ProductEntity>(sql);

                var entityList = entities.ToList();

                if (!entityList.Any())
                    return null;

                var models = entityList.Select(e => _productMapper.Map(e));

                return new Models.Products(models);
            }
        }

        public async Task<Models.Product> GetAsync(Guid id)
        {
            var sql = $@"SELECT * from {ProductTable} WHERE {nameof(ProductEntity.Id)}=@{nameof(id)} COLLATE NOCASE";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var entities = await conn.QueryAsync<ProductEntity>(sql, new { id });

                var entity = entities.FirstOrDefault();

                if (entity == null)
                    return null;

                return _productMapper.Map(entity);
            }
        }

        public async Task<ProductOptions> GetOptionsAsync(Guid id, string trackId)
        {
            var sql = $@"SELECT * from {ProductOptionTable} WHERE {nameof(ProductOptionEntity.ProductId)}=@{nameof(id)} COLLATE NOCASE";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var entities = await conn.QueryAsync<ProductOptionEntity>(sql, new { id });

                var entityList = entities.ToList();

                if (!entityList.Any())
                    return null;

                var models = entityList.Select(e => _optionMapper.Map(e));

                return new ProductOptions(models);
            }
        }

        public async Task<ProductOption> GetOptionAsync(Guid productId, Guid id, string trackId)
        {
            var sql = $@"SELECT * from {ProductOptionTable} WHERE {nameof(ProductOptionEntity.Id)}=@{nameof(id)} COLLATE NOCASE";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var entities = await conn.QueryAsync<ProductOptionEntity>(sql, new { id });

                var entity = entities.FirstOrDefault();

                if (entity == null)
                    return null;

                if (!entity.ProductId.Equals(productId.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    throw new BadRequestException($"An error occurred. Please review trackId [{trackId}] for more details", null, trackId);

                var model = _optionMapper.Map(entity);

                return model;
            }
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