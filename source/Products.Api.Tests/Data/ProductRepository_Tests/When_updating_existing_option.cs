using System;
using System.Threading.Tasks;
using Products.Api.Models;
using Products.Api.Models.Exceptions;
using Products.Api.TestFacilities;
using Xunit;
// ReSharper disable InconsistentNaming
namespace Products.Api.Data.ProductRepository_Tests
{
    public class When_updating_existing_option
    {
        private readonly ProductRepositoryTestFixture _fixture = new ProductRepositoryTestFixture();

        [Fact]
        public async Task Service_updates_the_option_if_it_exists()
        {
            // arrange 
            var sut = _fixture.Build();
            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).Build();
            await sut.AddAsync(option.ProductId, option, "1");

            // act
            option = new ProductOption(option.Id, option.ProductId, $"{option.Name} x", $"{option.Description} x");
            await sut.UpdateAsync(option.ProductId, option.Id, option, Guid.NewGuid().ToString());
            var option2 = await sut.GetOptionAsync(option.ProductId, option.Id, "1");


            // assert
            Assert.Equal(option.Id, option2.Id);
            Assert.Equal(option.ProductId, option2.ProductId);
            Assert.Equal(option.Name, option2.Name);
            Assert.Equal(option.Description, option2.Description);
        }

        [Fact]
        public async Task Service_throws_bad_request_if_the_option_does_not_exist()
        {
            // arrange 
            var sut = _fixture.Build();
            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).Build();

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.UpdateAsync(option.ProductId, option.Id, option, "1"));
        }

        [Fact]
        public async Task Service_throws_bad_request_if_the_option_does_not_belong_to_the_product()
        {
            // arrange 
            var sut = _fixture.Build();
            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).Build();

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.UpdateAsync(Guid.NewGuid(), option.Id, option, "1"));
        }

        [Fact]
        public async Task Service_throws_bad_request_if_the_option_Id_provided_does_not_match_with_id_of_the_option()
        {
            // arrange 
            var sut = _fixture.Build();
            var option = _fixture.OptionBuilder.Start().WithProduct(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).Build();

            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.UpdateAsync(option.ProductId, Guid.NewGuid(), option, "1"));
        }
    }
}