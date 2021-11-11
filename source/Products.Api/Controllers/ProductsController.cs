using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Data;
using Products.Api.Models;
using Products.Api.Models.Exceptions;

namespace Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var result = await _productRepository.GetAsync();

                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            try
            {
                var result = await _productRepository.GetAsync(id);
                if (result == null)
                    return new BadRequestObjectResult(new ErrorResponse( HttpContext.TraceIdentifier));

                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Product product)
        {
            try
            {
                await _productRepository.AddAsync(product, HttpContext.TraceIdentifier);

                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] Product product)
        {
            try
            {
                await _productRepository.UpdateAsync(id, product, HttpContext.TraceIdentifier);

                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
            /*var orig = new Product(id)
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };

            if (!orig.IsNew)
                orig.Save();

            await Task.FromResult(true);

            return new OkResult();*/
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var product = new Product(id);
            product.Delete();
        }

        [HttpGet("{productId}/options")]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        [HttpGet("{productId}/options/{id}")]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
                throw new Exception();

            return option;
        }

        [HttpPost("{productId}/options")]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [HttpPut("{productId}/options/{id}")]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [HttpDelete("{productId}/options/{id}")]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }
    }
}