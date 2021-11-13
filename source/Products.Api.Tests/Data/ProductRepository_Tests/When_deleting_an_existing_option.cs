using System;
using System.Threading.Tasks;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;
// ReSharper disable InconsistentNaming

namespace Products.Api.Data.ProductRepository_Tests
{
    public class When_deleting_an_existing_option
    {
        private readonly ProductRepositoryTestFixture _fixture = new ProductRepositoryTestFixture();

        [Fact]
        public async Task Service_deletes_the_option_if_it_exists()
        {
            // arrange
            var sut = _fixture.Build();
            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).Build();
            await sut.AddAsync(option.ProductId, option, Guid.NewGuid().ToString());

            // act 
            await sut.DeleteAsync(option.ProductId, option.Id, "1");
            var result = await sut.GetOptionAsync(option.ProductId, option.Id, "1");

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Service_throws_bad_request_exception_if_option_does_not_exist()
        {
            // arrange
            var sut = _fixture.Build();
            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).Build();

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async ()=>await sut.DeleteAsync(option.ProductId, option.Id, "1"));
        }

        [Fact]
        public async Task Service_throws_bad_request_exception_if_option_does_not_belong_to_the_product()
        {
            // arrange
            var sut = _fixture.Build();
            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).Build();
            await sut.AddAsync(option.ProductId, option, Guid.NewGuid().ToString());

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.DeleteAsync(Guid.NewGuid(), option.Id, "1"));
        }
    }
}