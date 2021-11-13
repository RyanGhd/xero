using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Models;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;

namespace Products.Api.Controllers.ProductsController_Tests
{
    // ReSharper disable once InconsistentNaming
    public class When_deleting_a_product
    {
        private readonly ProductsControllerTestFixture _fixture = new ProductsControllerTestFixture();

        [Fact]
        public async Task Service_deletes_the_product_using_product_Id()
        {
            // arrange 
            var sut = _fixture.Start().Build();

            // act
            var result = (OkResult) await sut.DeleteAsync(Guid.NewGuid());

            // assert
            Assert.Equal(200,result.StatusCode);
        }

        [Fact]
        public async Task Service_returns_400_if_it_can_not_delete_the_Product()
        {
            // arrange 
            var sut = _fixture.Start().WithSetupForDelete(new BadRequestException("111")).Build();

            // act
            var result = (BadRequestObjectResult) await sut.DeleteAsync(Guid.NewGuid());

            // assert
            Assert.Equal(400,result.StatusCode);
            Assert.NotNull(((ErrorResponse)result.Value).ErrorMessage);
        }
    }
}