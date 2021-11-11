using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Models;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;

namespace Products.Api.Controllers.ProductsController_Tests
{
    // ReSharper disable once InconsistentNaming
    public class When_updating_a_product
    {
        private readonly ProductsControllerTestFixture _fixture = new ProductsControllerTestFixture();

        [Fact]
        public async Task Service_updates_the_products_if_it_exists()
        {
            // arrange 
            var sut = _fixture.Start().Build();
            var product = _fixture.GetProduct();

            // act
            var result = (OkResult)await sut.UpdateAsync(product.Id, product);

            // assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Service_returns_400_if_an_error_occurs_in_the_update_process()
        {
            // arrange 
            var sut = _fixture.Start().WithSetupForAdd(new BadRequestException("Error", null, "111")).Build();
            var product = _fixture.GetProduct();

            // act
            var result = (BadRequestObjectResult)await sut.PostAsync(product);

            // assert
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(((ErrorResponse)result.Value).ErrorMessage);
        }
    }
}