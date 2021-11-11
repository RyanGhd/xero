using System;
using System.Linq;
using System.Threading.Tasks;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;
// ReSharper disable InconsistentNaming
namespace Products.Api.Data.ProductRepository_Tests
{
    public class When_getting_data
    {
        private ProductRepositoryTestFixture _fixture = new ProductRepositoryTestFixture();

        [Fact]
        public async Task Service_can_read_products_from_db()
        {
            // arrange 
            var sut = _fixture.Build();

            // act
            var result = await sut.GetAsync();

            // assert
            Assert.True(result.Items.Any());
        }

        [Fact]
        public async Task Service_can_read_one_product_using_Id_from_db()
        {
            // arrange 
            var sut = _fixture.Build();
            var id = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3");

            // act
            var result = await sut.GetAsync(id);

            // assert
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task Service_can_read_options_related_to_a_specific_product_using_product_id_from_db()
        {
            // arrange 
            var sut = _fixture.Build();
            var productId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3");

            // act
            var result = await sut.GetOptionsAsync(productId, Guid.NewGuid().ToString());

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            result.Items.ForEach(r => Assert.Equal(productId, r.ProductId));
        }

        [Fact]
        public async Task Service_can_read_one_option_related_to_a_specific_product_using_option_Id_from_db()
        {
            // arrange 
            var sut = _fixture.Build();
            var productId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3");
            var optionId = Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABCC9");

            // act  
            var result = await sut.GetOptionAsync(productId, optionId, Guid.NewGuid().ToString());

            // assert
            Assert.NotNull(result);
            Assert.Equal(optionId,result.Id);
            Assert.Equal(productId,result.ProductId);
        }

        [Fact]
        public async Task Service_throws_bad_request_if_the_requested_option_using_option_Id_does_not_belong_to_the_product()
        {
            // arrange 
            var sut = _fixture.Build();
            var productId = Guid.NewGuid();
            var optionId = Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABCC9");

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async ()=> await sut.GetOptionAsync(productId, optionId, Guid.NewGuid().ToString()));
        }
    }
}