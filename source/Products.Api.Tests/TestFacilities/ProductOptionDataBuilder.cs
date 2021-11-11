using System;
using System.Collections.Generic;
using Products.Api.Models;

namespace Products.Api.TestFacilities
{
    public class ProductOptionDataBuilder
    {
        Guid Id { get; set; }
        Guid ProductId { get; set; }
        string Name { get; set; }
        string Description { get; set; }

        public ProductOptionDataBuilder Start()
        {
            Id = Guid.NewGuid();
            ProductId = Guid.NewGuid();
            Name = $"P {DateTime.Now.Ticks}";
            Description = $"P {DateTime.Now.Ticks}";

            return this;
        }

        public ProductOptionDataBuilder WithProduct(Guid productId)
        {
            ProductId = productId;

            return this;
        }
        public ProductOption Build()
        {
            return new ProductOption(Id, ProductId, Name, Description);

        }

    }
}