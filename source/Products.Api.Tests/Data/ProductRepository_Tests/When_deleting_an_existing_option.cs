using System;
using System.Threading.Tasks;
using Xunit;
// ReSharper disable InconsistentNaming

namespace Products.Api.Data.ProductRepository_Tests
{
    public class When_deleting_an_existing_option
    {
        [Fact]
        public Task Service_deletes_the_option_if_it_exists()
        {
            throw new NotImplementedException();
        }
        
        [Fact]
        public Task Service_throws_bad_request_exception_if_option_does_not_exist()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public Task Service_throws_bad_request_exception_if_option_does_not_belong_to_the_product()
        {
            throw new NotImplementedException();
        }
    }
}