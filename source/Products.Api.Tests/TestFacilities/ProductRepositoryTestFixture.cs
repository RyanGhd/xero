using System;
using Products.Api.Data;
using Products.Api.Data.Mappers;
using Products.Api.Models;

namespace Products.Api.TestFacilities
{
    public class ProductRepositoryTestFixture
    {
        private readonly ProductDataBuilder _productBuilder = new ProductDataBuilder();
        public ProductOptionDataBuilder OptionBuilder { get; }

        private readonly IDbConnectionFactory _connectionFactory = new DbConnectionFactory(new AppSettings("Data Source=App_Data/products.db"));
        private readonly IProductToProductEntityMapper _productMapper = new ProductToProductEntityMapper();
        private readonly IProductOptionToProductOptionEntityMapper _optionMapper = new ProductOptionToProductOptionEntityMapper();

        public ProductRepositoryTestFixture()
        {
            OptionBuilder = new ProductOptionDataBuilder();
        }

        public ProductRepository Build()
        {
            return new ProductRepository(_connectionFactory, _productMapper, _optionMapper);
        }

        public Models.Product GetProduct()
        {
            return _productBuilder.Start().Build();
        }

        public ProductOption GetOption()
        {
            return OptionBuilder.Start().Build();
        }
    }
}