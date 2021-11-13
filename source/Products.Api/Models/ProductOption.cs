using System;

namespace Products.Api.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductOption(Guid id, Guid productId, string name, string description)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            Description = description;
        }
    }
}