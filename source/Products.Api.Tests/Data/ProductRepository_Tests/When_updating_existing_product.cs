using System;
using System.Threading.Tasks;
using Products.Api.Models;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;
// ReSharper disable InconsistentNaming
namespace Products.Api.Data.ProductRepository_Tests
{
    public class When_updating_existing_product
    {
        private readonly ProductRepositoryTestFixture _fixture = new ProductRepositoryTestFixture();

        [Fact]
        public async Task Service_updates_the_product_if_it_exists()
        {
            // arrange 
            var sut = _fixture.Build();
            var product = await sut.GetAsync(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"));

            // act
            product = new Product(product.Id, $"{product.Name} x", $"{product.Description} x", product.Price * 10, product.DeliveryPrice * 10);

            await sut.UpdateAsync(product.Id, product, Guid.NewGuid().ToString());
            var product2 = await sut.GetAsync(product.Id);

            // assert
            Assert.Equal(product2.Id, product.Id);
            Assert.Equal(product2.Name, product.Name);
            Assert.Equal(product2.Description, product.Description);
            Assert.Equal(product2.Price, product.Price);
            Assert.Equal(product2.DeliveryPrice, product.DeliveryPrice);
        }

        [Fact]
        public async Task Service_throws_bad_request_exception_if_the_product_does_not_exist()
        {
            // arrange 
            var sut = _fixture.Build();
            var product = _fixture.GetProduct();

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.UpdateAsync(product.Id, product, Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task Service_throws_bad_request_exception_if_the_productId_does_not_match_with_the_id_of_the_product()
        {
            // arrange 
            var sut = _fixture.Build();
            var product = _fixture.GetProduct();

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.UpdateAsync(Guid.NewGuid(), product, Guid.NewGuid().ToString()));
        }

    }
}