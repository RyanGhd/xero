using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Models;
using Products.Api.TestFacilities;
using Xunit;

namespace Products.Api.Controllers.ProductsController_Tests
{
    public class When_getting_one_product
    {
        private readonly ProductsControllerTestFixture _fixture = new ProductsControllerTestFixture();

        [Fact]
        public async Task Service_returns_the_product_if_it_exists()
        {
            // arrange 
            var inputProducts = _fixture.GetProducts();
            var inputProduct = inputProducts.Items.First();

            var sut = _fixture.Start().WithProduct(inputProduct).Build();

            // act 
            var result = (OkObjectResult)await sut.GetAsync(inputProduct.Id);
            var products = (Models.Product)result.Value;

            // assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(inputProduct.Id, products.Id);
        }

        [Fact]
        public async Task Service_returns_404_if_product_does_not_exist()
        {
            // arrange 
            var sut = _fixture.Start().WithNoProduct().Build();

            // act 
            var result = (NotFoundObjectResult)await sut.GetAsync(Guid.NewGuid());
            var error = (NotFoundErrorResponse)result.Value;

            // assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Product not found", error.ErrorMessage);
            Assert.NotNull(error.TrackId);

        }
    }
}