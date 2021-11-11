using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Models;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;

// ReSharper disable InconsistentNaming

namespace Products.Api.Controllers.ProductsController_Tests
{
    public class When_getting_all_of_products
    {
        private readonly ProductsControllerTestFixture _fixture = new ProductsControllerTestFixture();

        [Fact]
        public async Task Service_returns_successful_response_If_there_is_no_product()
        {
            // arrange 
            var sut = _fixture.Start().WithSetupForGet(new Models.Products(new List<Product>())).Build();

            // act 
            var result = (OkObjectResult)await sut.GetAsync();
            var products = (Models.Products)result.Value;

            // assert
            Assert.Equal(200, result.StatusCode);
            Assert.Empty(products.Items);
        }

        [Fact]
        public async Task Service_can_get_list_of_all_products()
        {
            // arrange 
            var inputProducts = _fixture.GetProducts();
            var sut = _fixture.Start().WithSetupForGet(inputProducts).Build();

            // act 
            var result = (OkObjectResult)await sut.GetAsync();
            var products = (Models.Products)result.Value;

            // assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(inputProducts.Items.Count(),products.Items.Count());
            Assert.True(inputProducts.Items.All(ip => products.Items.Any(p => p.Id == ip.Id)));
        }

        [Fact]
        public async Task Service_returns_400_if_an_error_happens()
        {
            // arrange 
            var sut = _fixture.Start().WithSetupForGet(new BadRequestException("Error", null, "111")).Build();

            // act
            var result = (BadRequestObjectResult)await sut.GetAsync();

            // assert
            Assert.Equal(400, result.StatusCode);
            Assert.NotNull(((ErrorResponse)result.Value).ErrorMessage);
        }
    }
}