using System;
using Products.Api.TestFacilities;
using Xunit;
using Xunit.Sdk;
// ReSharper disable InconsistentNaming

namespace Products.Api.Data.Mappers
{
    public class ProductOptionToProductOptionEntityMapper_Tests
    {
        private readonly ProductOptionDataBuilder _dataBuilder = new ProductOptionDataBuilder();

        [Fact]
        public void Service_maps_model_to_entity()
        {
            // arrange 
            var sut = new ProductOptionToProductOptionEntityMapper();
            var option = _dataBuilder.Start().Build();

            // act
            var entity = sut.Map(option);

            // assert
            Assert.Equal(option.Id, Guid.Parse(entity.Id));
            Assert.Equal(option.ProductId, Guid.Parse(entity.ProductId));
            Assert.Equal(option.Name, entity.Name);
            Assert.Equal(option.Description, entity.Description);
        }

        [Fact]
        public void Service_maps_entity_to_model()
        {
            // arrange 
            var sut = new ProductOptionToProductOptionEntityMapper();
            var option = _dataBuilder.Start().Build();
            var entity = sut.Map(option);

            // act
            var option2 = sut.Map(entity);

            // assert
            Assert.Equal(option2.Id, Guid.Parse(entity.Id));
            Assert.Equal(option2.ProductId, Guid.Parse(entity.ProductId));
            Assert.Equal(option2.Name, entity.Name);
            Assert.Equal(option2.Description, entity.Description);
        }
    }
}