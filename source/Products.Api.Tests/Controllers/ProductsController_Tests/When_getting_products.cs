using System;
using System.Linq;
using System.Threading.Tasks;
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
            var result = sut.Get();

            // assert
            Assert.True(result.Items.Any());
        }
    }
}