using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;
// ReSharper disable InconsistentNaming

namespace Products.Api.Data.ProductRepository_Tests
{
    public class When_deleting_an_existing_product
    {
        private readonly ProductRepositoryTestFixture _fixture = new ProductRepositoryTestFixture();

        [Fact]
        public async Task Service_deletes_the_product()
        {
            // arrange
            var sut = _fixture.Build();
            var product = _fixture.GetProduct();
            await sut.AddAsync(product, "1");

            // act 
            await sut.DeleteAsync(product.Id, "1");
            var result = await sut.GetAsync(product.Id);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Service_throws_bad_request_exception_if_product_does_not_exist()
        {
            // arrange
            var sut = _fixture.Build();
            var product = _fixture.GetProduct();

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.DeleteAsync(product.Id, "1"));
        }
    }
}