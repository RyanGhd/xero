using System;
using Products.Api.TestFacilities;
using Xunit;
using Xunit.Sdk;
// ReSharper disable InconsistentNaming

namespace Products.Api.Data.Mappers
{
    public class ProductToProductEntityMapper_Tests
    {
        private readonly ProductDataBuilder _dataBuilder = new ProductDataBuilder();

        [Fact]
        public void Service_maps_model_to_entity()
        {
            // arrange 
            var sut = new ProductToProductEntityMapper();
            var product = _dataBuilder.Start().Build();

            // act 
            var entity = sut.Map(product);

            // assert
            Assert.Equal(product.Id, Guid.Parse(entity.Id));
            Assert.Equal(product.Name, entity.Name);
            Assert.Equal(product.Description, entity.Description);
            Assert.Equal(product.Price, entity.Price);
            Assert.Equal(product.DeliveryPrice, entity.DeliveryPrice);
        }

        [Fact]
        public void Service_maps_entity_to_model()
        {
            // arrange 
            var sut = new ProductToProductEntityMapper();
            var product = _dataBuilder.Start().Build();
            var entity = sut.Map(product);
            // act 
            var product2 = sut.Map(entity);

            // assert
            Assert.Equal(product2.Id, Guid.Parse(entity.Id));
            Assert.Equal(product2.Name, entity.Name);
            Assert.Equal(product2.Description, entity.Description);
            Assert.Equal(product2.Price, entity.Price);
            Assert.Equal(product2.DeliveryPrice, entity.DeliveryPrice);
        }
    }
}