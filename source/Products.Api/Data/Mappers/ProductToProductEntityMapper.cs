using System;
using System.Globalization;
using Products.Api.Data.Entities;
using Products.Api.Models;

namespace Products.Api.Data.Mappers
{
    public interface IProductToProductEntityMapper
    {
        Product Map(ProductEntity entity);
        ProductEntity Map(Product model);
    }
    public class ProductToProductEntityMapper : IProductToProductEntityMapper
    {
        public Product Map(ProductEntity entity)
        {
            return new Product(Guid.Parse(entity.Id), entity.Name, entity.Description, entity.Price, entity.DeliveryPrice);
        }

        public ProductEntity Map(Product model)
        {
            return new ProductEntity
            {
                Id = model.Id.ToString(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                DeliveryPrice = model.DeliveryPrice
            };
        }
    }
}