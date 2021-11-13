using System;
using System.Threading.Tasks;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;
// ReSharper disable InconsistentNaming
namespace Products.Api.Data.ProductRepository_Tests
{
    public class When_adding_new_product
    {
        private readonly ProductRepositoryTestFixture _fixture = new ProductRepositoryTestFixture();

        [Fact]
        public async Task Service_adds_new_product()
        {
            // arrange 
            var sut = _fixture.Build();
            var product = _fixture.GetProduct();

            // act
            await sut.AddAsync(product, Guid.NewGuid().ToString());

            var insertedProduct = await sut.GetAsync(product.Id);

            // assert
            Assert.Equal(product.Id, insertedProduct.Id);
            Assert.Equal(product.Name, insertedProduct.Name);
            Assert.Equal(product.Description, insertedProduct.Description);
            Assert.Equal(product.Price, insertedProduct.Price);
            Assert.Equal(product.DeliveryPrice, insertedProduct.DeliveryPrice);
        }

        [Fact]
        public async Task Service_throws_bad_request_if_the_product_already_exists()
        {
            // arrange 
            var sut = _fixture.Build();
            var product = _fixture.GetProduct();

            // act
            await sut.AddAsync(product, Guid.NewGuid().ToString());

            // assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.AddAsync(product, "1"));
        }
    }
}