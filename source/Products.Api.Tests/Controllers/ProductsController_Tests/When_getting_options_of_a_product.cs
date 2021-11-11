using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Models;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;

namespace Products.Api.Controllers.ProductsController_Tests
{
    // ReSharper disable once InconsistentNaming
    public class When_getting_options_of_a_product
    {
        private readonly ProductsControllerTestFixture _fixture = new ProductsControllerTestFixture();

        [Fact]
        public async Task Service_returns_the_options_using_product_id()
        {
            // arrange 
            var inputOptions = _fixture.GetOptions();

            var sut = _fixture.Start().WithSetupForGetOptions(inputOptions).Build();

            // act 
            var result = (OkObjectResult)await sut.GetOptionsAsync(Guid.NewGuid());
            var options = (Models.ProductOptions)result.Value;

            // assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(inputOptions.Items.Count, options.Items.Count);
            Assert.True(inputOptions.Items.All(io => options.Items.Any(o => o.Id.Equals(io.Id))));
        }

        [Fact]
        public async Task Service_returns_400_if_product_provided_for_the_options_does_not_exist()
        {
            // arrange 
            var sut = _fixture.Start().WithSetupForNoOptions().Build();

            // act 
            var result = (BadRequestObjectResult)await sut.GetOptionsAsync(Guid.NewGuid());
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
            var sut = _fixture.Start().WithSetupForGetOptions(new BadRequestException("Error", null, "111")).Build();

            // act
            var result = (BadRequestObjectResult)await sut.GetOptionsAsync(Guid.NewGuid());

            // assert
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(((ErrorResponse)result.Value).ErrorMessage);
        }
    }
}