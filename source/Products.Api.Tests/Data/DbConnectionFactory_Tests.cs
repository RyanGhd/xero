using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Products.Api.Models;
using Xunit;

namespace Products.Api.Data
{
   public class DbConnectionFactory_Tests
    {
        [Fact]
        public void Service_can_connect_to_the_db()
        {
            //arrange 
            var settings = new AppSettings("Data Source=App_Data/products.db");
            var sut = new DbConnectionFactory(settings);

            // act & assert
            using (var conn = sut.BuildConnection())
            {
                conn.Open();
            }
        }
    }
}
