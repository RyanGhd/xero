using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Models;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;

namespace Products.Api.Controllers.ProductsController_Tests
{
    // ReSharper disable once InconsistentNaming
    public class When_adding_a_new_product
    {
        private readonly ProductsControllerTestFixture _fixture = new ProductsControllerTestFixture();

        [Fact]
        public async Task Service_adds_new_product_to_the_list_of_products()
        {
            // arrange 
            var sut = _fixture.Start().Build();
            var product = _fixture.GetProduct();

            // act
            var result = (OkResult) await sut.PostAsync(product);

            // assert
            Assert.Equal(200,result.StatusCode);
        }

        [Fact]
        public async Task Service_returns_400_if_it_can_not_add_the_Product()
        {
            // arrange 
            var sut = _fixture.Start().WithSetupForAdd(new BadRequestException("111")).Build();
            var product = _fixture.GetProduct();

            // act
            var result = (BadRequestObjectResult) await sut.PostAsync(product);

            // assert
            Assert.Equal(400,result.StatusCode);
            Assert.NotNull(((ErrorResponse)result.Value).ErrorMessage);
        }
    }
}