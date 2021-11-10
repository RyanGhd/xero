using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Api.TestFacilities;
using Xunit;

namespace Products.Api.Controllers.ProductsController_Tests
{
    // ReSharper disable once InconsistentNaming
    public class When_adding_new_product
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
    }
}