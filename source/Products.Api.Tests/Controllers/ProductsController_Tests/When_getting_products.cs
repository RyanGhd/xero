using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Api.TestFacilities;
using Xunit;
// ReSharper disable InconsistentNaming

namespace Products.Api.Controllers.ProductsController_Tests
{
    public class When_getting_products
    {
        private readonly ProductsControllerTestFixture _fixture = new ProductsControllerTestFixture();

        [Fact]
        public async Task Service_can_get_list_of_all_prodcuts()
        {
            // arrange 
            var sut = _fixture.Start().Build();

            // act 
            var result = (OkObjectResult)await sut.GetAsync();
            var products = (Models.Products)result.Value;

            // assert
            Assert.Equal(200, result.StatusCode);
            Assert.True(products.Items.Any());
        }
    }
}