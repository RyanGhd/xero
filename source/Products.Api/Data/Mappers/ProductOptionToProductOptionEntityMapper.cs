using System;
using Products.Api.Data.Entities;
using Products.Api.Models;

namespace Products.Api.Data.Mappers
{
    public interface IProductOptionToProductOptionEntityMapper
    {
        ProductOption Map(ProductOptionEntity entity);
        ProductOptionEntity Map(ProductOption model);
    }
    public class ProductOptionToProductOptionEntityMapper : IProductOptionToProductOptionEntityMapper
    {
        public ProductOption Map(ProductOptionEntity entity)
        {
            return new ProductOption(Guid.Parse(entity.Id), Guid.Parse(entity.ProductId), entity.Name, entity.Description);
        }

        public ProductOptionEntity Map(ProductOption model)
        {
            return new ProductOptionEntity
            {
                Id = model.Id.ToString(),
                ProductId = model.ProductId.ToString(),
                Description = model.Description,
                Name = model.Name
            };
        }
    }
}