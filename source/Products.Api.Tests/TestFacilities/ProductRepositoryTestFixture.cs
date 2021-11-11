using System;
using Products.Api.Data;
using Products.Api.Data.Mappers;
using Products.Api.Models;

namespace Products.Api.TestFacilities
{
    public class ProductRepositoryTestFixture
    {
        private readonly IDbConnectionFactory _connectionFactory = new DbConnectionFactory(new AppSettings("Data Source=App_Data/products.db"));
        private readonly IProductToProductEntityMapper _productMapper = new ProductToProductEntityMapper();
        private readonly IProductOptionToProductOptionEntityMapper _optionMapper = new ProductOptionToProductOptionEntityMapper();


        public ProductRepository Build()
        {
            return new ProductRepository(_connectionFactory, _productMapper,_optionMapper);
        }
    }
}