using System;
using Products.Api.Models;

namespace Products.Api.TestFacilities
{
    public class ProductDataBuilder
    {
          Guid Id { get; set; }
          string Name { get; set; }
          string Description { get; set; }
          decimal Price { get; set; }
          decimal DeliveryPrice { get; set; }
     

        public ProductDataBuilder Start()
        {
            Id = Guid.NewGuid();
            Name = $"Product {DateTime.Now.Minute}";
            Description = Name;
            Price = (new Random()).Next(10,10000);
            DeliveryPrice = Price * 1.1M;

            return this;
        }

        public Product Build()
        {
            return new Product
            {
                Id = Id, 
                Name = Name,
                Description = Description,
                Price = Price,
                DeliveryPrice = DeliveryPrice
            };
        }
    }
}