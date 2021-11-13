using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Products.Api.Data.Entities;
using Products.Api.Data.Mappers;
using Products.Api.Models;
using Products.Api.Models.Exceptions;
using IsolationLevel = System.Data.IsolationLevel;

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
            var sql = GetBaseSelectSql();

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
            var sql = $@"{GetBaseSelectSql()}
                         WHERE {nameof(ProductEntity.Id)}=@{nameof(id)} COLLATE NOCASE";

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
                    throw new BadRequestException(new Exception("Product option does not belong to the product"), trackId);

                var model = _optionMapper.Map(entity);

                return model;
            }
        }

        public async Task AddAsync(Product product, string trackId)
        {
            var sql = $@"SELECT COUNT(*) FROM {ProductTable} WHERE {nameof(ProductEntity.Id)}=@{nameof(ProductEntity.Id)}  COLLATE NOCASE";

            var insertSql =
                $@"INSERT INTO {ProductTable} (
                   {nameof(ProductEntity.Id)},
                   {nameof(ProductEntity.Name)},
                   {nameof(ProductEntity.Description)},
                   {nameof(ProductEntity.Price)},
                   {nameof(ProductEntity.DeliveryPrice)}
                   )
                   VALUES(
                   @{nameof(ProductEntity.Id)},
                   @{nameof(ProductEntity.Name)},
                   @{nameof(ProductEntity.Description)},
                   @{nameof(ProductEntity.Price)},
                   @{nameof(ProductEntity.DeliveryPrice)}
                   )";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var count = await conn.QuerySingleOrDefaultAsync<int>(sql, new { product.Id });

                if (count > 0)
                    throw new BadRequestException(new Exception("Product already exist"), trackId);

                var result = await conn.ExecuteAsync(insertSql, product);

                if (result == 0)
                    throw new BadRequestException(new Exception("Failed to insert product record"), trackId);
            }
        }

        public async Task AddAsync(Guid productId, ProductOption option, string trackId)
        {
            if (!productId.Equals(option.ProductId))
                throw new BadRequestException(new Exception("Product option does not belong to the product"), trackId);

            var productSql = $@"SELECT COUNT(*) FROM {ProductTable} WHERE {nameof(ProductEntity.Id)}=@{nameof(productId)}  COLLATE NOCASE";

            var sql = $@"SELECT COUNT(*) FROM {ProductOptionTable} WHERE {nameof(ProductOptionEntity.Id)}=@{nameof(ProductOptionEntity.Id)}  COLLATE NOCASE";

            var insertSql =
                $@"INSERT INTO {ProductOptionTable} (
                   {nameof(ProductOptionEntity.Id)},
                   {nameof(ProductOptionEntity.ProductId)},
                   {nameof(ProductOptionEntity.Name)},
                   {nameof(ProductOptionEntity.Description)}
                   )
                   VALUES(
                   @{nameof(ProductOptionEntity.Id)},
                   @{nameof(ProductOptionEntity.ProductId)},
                   @{nameof(ProductOptionEntity.Name)},
                   @{nameof(ProductOptionEntity.Description)}
                   )";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var productCount = await conn.QuerySingleOrDefaultAsync<int>(productSql, new { productId });

                if (productCount == 0)
                    throw new BadRequestException(new Exception("Product does not exist"), trackId);

                var optionCount = await conn.QuerySingleOrDefaultAsync<int>(sql, new { option.Id });

                if (optionCount > 0)
                    throw new BadRequestException(new Exception("Product option already exists in the DB"), trackId);

                var result = await conn.ExecuteAsync(insertSql, option);

                if (result == 0)
                    throw new BadRequestException(new Exception("Failed to insert the product option record"), trackId);
            }
        }

        public async Task UpdateAsync(Guid id, Product product, string trackId)
        {
            if (!id.Equals(product.Id))
                throw new BadRequestException(new Exception("Id does not match with ProductId"), trackId);

            var sql = $@"SELECT COUNT(*) FROM {ProductTable} WHERE {nameof(ProductEntity.Id)}=@{nameof(id)}  COLLATE NOCASE";

            var updateSql =
                $@"UPDATE {ProductTable} 
                   SET
                   {nameof(ProductEntity.Id)}=@{nameof(ProductEntity.Id)},
                   {nameof(ProductEntity.Name)}=@{nameof(ProductEntity.Name)},
                   {nameof(ProductEntity.Description)}=@{nameof(ProductEntity.Description)},
                   {nameof(ProductEntity.Price)}=@{nameof(ProductEntity.Price)},
                   {nameof(ProductEntity.DeliveryPrice)}=@{nameof(ProductEntity.DeliveryPrice)}
                   WHERE {nameof(ProductEntity.Id)}=@{nameof(ProductEntity.Id)}";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var count = await conn.QuerySingleOrDefaultAsync<int>(sql, new { id });

                if (count ==0)
                    throw new BadRequestException(new Exception("Product does not exist"), trackId);

                var result = await conn.ExecuteAsync(updateSql, product);

                if (result == 0)
                    throw new BadRequestException(new Exception("Failed to update product record"), trackId);
            }
        }

        public async Task UpdateAsync(Guid productId, Guid id, ProductOption option, string trackId)
        {
            if (!id.Equals(option.Id))
                throw new BadRequestException(new Exception("Id does not match with Option Id"), trackId);

            if (!productId.Equals(option.ProductId))
                throw new BadRequestException(new Exception("ProductId does not match with Option ProductId"), trackId);

            var sql = $@"SELECT COUNT(*) FROM {ProductOptionTable} WHERE {nameof(ProductOptionEntity.Id)}=@{nameof(id)}  COLLATE NOCASE";

            var updateSql =
                $@"UPDATE {ProductOptionTable} 
                   SET
                   {nameof(ProductOptionEntity.Id)}=@{nameof(ProductOptionEntity.Id)},
                   {nameof(ProductOptionEntity.ProductId)}=@{nameof(ProductOptionEntity.ProductId)},
                   {nameof(ProductOptionEntity.Name)}=@{nameof(ProductOptionEntity.Name)},
                   {nameof(ProductOptionEntity.Description)}=@{nameof(ProductOptionEntity.Description)}
                   WHERE {nameof(ProductOptionEntity.Id)}=@{nameof(ProductOptionEntity.Id)}";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var count = await conn.QuerySingleOrDefaultAsync<int>(sql, new { id });

                if (count == 0)
                    throw new BadRequestException(new Exception("Product option does not exist"), trackId);

                var result = await conn.ExecuteAsync(updateSql, option);

                if (result == 0)
                    throw new BadRequestException(new Exception("Failed to update product option record"), trackId);
            }
        }

        public async Task DeleteAsync(Guid id, string trackId)
        {
            var sql =
                $@"DELETE FROM {ProductTable} WHERE {nameof(ProductEntity.Id)}=@{nameof(id)} COLLATE NOCASE";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var result = await conn.ExecuteAsync(sql, new { id });

                if (result == 0)
                    throw new BadRequestException(new Exception("Failed to delete the product option record"), trackId);
            }
        }

        public async Task DeleteAsync(Guid productId, Guid id, string trackId)
        {
            var sql =
                $@"DELETE FROM {ProductOptionTable} 
                   WHERE {nameof(ProductOptionEntity.Id)}=@{nameof(id)} AND
                         {nameof(ProductOptionEntity.ProductId)}=@{nameof(productId)} COLLATE NOCASE";

            using (var conn = _connectionFactory.BuildConnection())
            {
                var result = await conn.ExecuteAsync(sql, new { productId, id });

                if (result == 0)
                    throw new BadRequestException(new Exception("Failed to delete the product option record"), trackId);
            }
        }

        private string GetBaseSelectSql()
        {
            return $@"SELECT 
                      {nameof(ProductEntity.Id)},
                      {nameof(ProductEntity.Name)},
                      {nameof(ProductEntity.Description)},
                      CAST({nameof(ProductEntity.Price)} as DOUBLE) AS {nameof(ProductEntity.Price)},
                      CAST({nameof(ProductEntity.DeliveryPrice)} as DOUBLE) AS {nameof(ProductEntity.DeliveryPrice)}
                      FROM {ProductTable}";
        }
    }
}