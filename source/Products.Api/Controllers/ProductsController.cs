using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Api.Data;
using Products.Api.Models;
using Products.Api.Models.Exceptions;

namespace Products.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> _logger)
        {
            _productRepository = productRepository;
            this._logger = _logger;
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
                _logger.LogError(e, null);
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                var result = await _productRepository.GetAsync(id);
                if (result == null)
                {
                    _logger.LogError($"Product does not exist. trackId: {HttpContext.TraceIdentifier}");

                    return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
                }

                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, null);
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
                _logger.LogError(e, null);
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] Product product)
        {
            try
            {
                await _productRepository.UpdateAsync(id, product, HttpContext.TraceIdentifier);

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, null);
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            try
            {
                await _productRepository.DeleteAsync(id, HttpContext.TraceIdentifier);

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, null);
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }

        [HttpGet("{productId}/options")]
        public async Task<IActionResult> GetOptionsAsync([FromRoute] Guid productId)
        {
            try
            {
                var options = await _productRepository.GetOptionsAsync(productId, HttpContext.TraceIdentifier);
                if (options == null)
                {
                    _logger.LogError($"Option does not exist. trackId: {HttpContext.TraceIdentifier}");

                    return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
                }

                return new OkObjectResult(options);
            }
            catch (Exception e)
            {
                _logger.LogError(e, null);
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }

        [HttpGet("{productId}/options/{id}")]
        public async Task<IActionResult> GetOptionAsync([FromRoute] Guid productId, [FromRoute] Guid id)
        {
            try
            {
                var option = await _productRepository.GetOptionAsync(productId, id, HttpContext.TraceIdentifier);
                if (option == null)
                {
                    _logger.LogError($"Option does not exist. trackId: {HttpContext.TraceIdentifier}");

                    return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
                }

                return new OkObjectResult(option);
            }
            catch (Exception e)
            {
                _logger.LogError(e, null);
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }

        [HttpPost("{productId}/options")]
        public async Task<IActionResult> CreateOptionAsync([FromRoute] Guid productId, [FromBody] ProductOption option)
        {
            try
            {
                await _productRepository.AddAsync(productId, option, HttpContext.TraceIdentifier);

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, null);
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }

        [HttpPut("{productId}/options/{id}")]
        public async Task<IActionResult> UpdateOptionAsync([FromRoute] Guid productId, [FromRoute] Guid id, [FromRoute] ProductOption option)
        {
            try
            {
                await _productRepository.UpdateAsync(productId, id, option, HttpContext.TraceIdentifier);

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, null);
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }

        [HttpDelete("{productId}/options/{id}")]
        public async Task<IActionResult> DeleteOptionAsync([FromRoute] Guid productId, [FromRoute] Guid id)
        {
            try
            {
                await _productRepository.DeleteAsync(productId, id, HttpContext.TraceIdentifier);

                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, null);
                return new BadRequestObjectResult(new ErrorResponse(HttpContext.TraceIdentifier));
            }
        }
    }
}