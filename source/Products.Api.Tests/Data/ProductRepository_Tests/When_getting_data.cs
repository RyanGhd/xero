using System;
using System.Linq;
using System.Threading.Tasks;
using Products.Api.TestFacilities;
using Xunit;

namespace Products.Api.Data.ProductRepository_Tests
{
    public class When_getting_data
    {
        private ProductRepositoryTestFixture _fixture = new ProductRepositoryTestFixture();

        [Fact]
        public async Task Service_can_read_products_from_db()
        {
            // arrange 
            var sut = _fixture.Start().Build();

            // act
            var result = await sut.GetAsync();

            // assert
            Assert.True(result.Items.Any());
        }

        [Fact]
        public async Task Service_can_read_one_product_by_Id_from_db()
        {
            // arrange 
            var sut = _fixture.Start().Build();
            var id = Guid.NewGuid();

            // act
            var result = await sut.GetAsync(id);

            // assert
            Assert.Equal(id, result.Id);
        }
    }
}