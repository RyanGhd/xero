using System;
using System.Threading.Tasks;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;
// ReSharper disable InconsistentNaming
namespace Products.Api.Data.ProductRepository_Tests
{
    public class When_adding_new_product_option
    {
        private readonly ProductRepositoryTestFixture _fixture = new ProductRepositoryTestFixture();

        [Fact]
        public async Task Service_adds_new_option_to_the_product()
        {
            // arrange 
            var sut = _fixture.Build();
            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).Build();

            // act
            await sut.AddAsync(option.ProductId, option, Guid.NewGuid().ToString());

            var insertedOption = await sut.GetOptionAsync(option.ProductId, option.Id, "1");

            // assert
            Assert.Equal(option.Id, insertedOption.Id);
            Assert.Equal(option.ProductId, insertedOption.ProductId);
            Assert.Equal(option.Name, insertedOption.Name);
            Assert.Equal(option.Description, insertedOption.Description);
        }

        [Fact]
        public async Task Service_throws_bad_request_if_product_does_not_exist()
        {
            // arrange 
            var sut = _fixture.Build();
            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.NewGuid()).Build();

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.AddAsync(option.ProductId, option, "1"));
        }

        [Fact]
        public async Task Service_throws_bad_request_if_product_option_does_not_belong_to_product()
        {
            // arrange 
            var sut = _fixture.Build();
            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.NewGuid()).Build();

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.AddAsync(Guid.NewGuid(), option, "1"));
        }

        [Fact]
        public async Task Service_throws_bad_request_if_option_already_exists()
        {
            // arrange 
            var sut = _fixture.Build();

            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).Build();

            // act
            await sut.AddAsync(option.ProductId, option, Guid.NewGuid().ToString());

            // assert & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.AddAsync(option.ProductId, option, "1"));
        }
    }
}