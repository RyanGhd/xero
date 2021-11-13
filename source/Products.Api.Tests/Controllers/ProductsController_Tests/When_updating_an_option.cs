using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Models;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;

namespace Products.Api.Controllers.ProductsController_Tests
{
    // ReSharper disable once InconsistentNaming
    public class When_updating_an_option
    {
        private readonly ProductsControllerTestFixture _fixture = new ProductsControllerTestFixture();

        [Fact]
        public async Task Service_updates_the_option_if_it_exists()
        {
            // arrange 
            var sut = _fixture.Start().Build();
            var inputOption = _fixture.GetOption();

            // act
            var result = (OkResult)await sut.UpdateOptionAsync(inputOption.ProductId,inputOption.Id,inputOption);

            // assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Service_returns_400_if_an_error_occurs_in_the_update_process()
        {
            // arrange 
            var sut = _fixture.Start().WithSetupForUpdate(new BadRequestException("111")).Build();
            var inputOption = _fixture.GetOption();

            // act
            var result = (BadRequestObjectResult)await sut.UpdateOptionAsync(inputOption.ProductId, inputOption.Id, inputOption);

            // assert
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(((ErrorResponse)result.Value).ErrorMessage);
        }
    }
}