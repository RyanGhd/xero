using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Models;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;

namespace Products.Api.Controllers.ProductsController_Tests
{
    public class When_getting_one_option_of_a_product
    {
        private readonly ProductsControllerTestFixture _fixture = new ProductsControllerTestFixture();

        [Fact]
        public async Task Service_returns_the_options_using_product_id()
        {
            // arrange 
            var inputOption = _fixture.GetOption();

            var sut = _fixture.Start().WithSetupForGetOption(inputOption).Build();

            // act 
            var result = (OkObjectResult)await sut.GetOptionAsync(Guid.NewGuid(), Guid.NewGuid());
            var option = (Models.ProductOption)result.Value;

            // assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(inputOption.Id, option.Id);
            Assert.Equal(inputOption.ProductId, option.ProductId);
            Assert.Equal(inputOption.Name, option.Name);
            Assert.Equal(inputOption.Description, option.Description);
        }

        [Fact]
        public async Task Service_returns_400_if_product_provided_for_the_option_or_option_itself_does_not_exist()
        {
            // arrange 
            var sut = _fixture.Start().WithSetupForNoOptions().Build();

            // act 
            var result = (BadRequestObjectResult)await sut.GetOptionAsync(Guid.NewGuid(),Guid.NewGuid());
            var error = (ErrorResponse)result.Value;

            // assert
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(error.ErrorMessage);
            Assert.NotNull(error.TrackId);
        }

        [Fact]
        public async Task Service_returns_400_if_an_error_happens()
        {
            // arrange 
            var sut = _fixture.Start().WithSetupForGetOption(new BadRequestException("111")).Build();

            // act
            var result = (BadRequestObjectResult)await sut.GetOptionAsync(Guid.NewGuid(),Guid.NewGuid());

            // assert
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(((ErrorResponse)result.Value).ErrorMessage);
        }
    }
}