using System;
using System.Threading.Tasks;
using HttpContextMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Products.Api.Controllers;
using Products.Api.Data;
using Products.Api.Models;
using Products.Api.Models.Exceptions;

namespace Products.Api.TestFacilities
{
    public class ProductsControllerTestFixture
    {
        private readonly ProductDataBuilder _productBuilder = new ProductDataBuilder();
        private readonly ProductOptionDataBuilder _optionBuilder = new ProductOptionDataBuilder();

        private HttpContextMock HttpContextMock { get; set; }
        private Mock<IProductRepository> ProductRepositoryMock { get; set; }
        private ILogger<ProductsController> Logger { get; set; }

        public ProductsControllerTestFixture Start()
        {
            HttpContextMock = new HttpContextMock();
            HttpContextMock.Mock.Setup(c => c.TraceIdentifier).Returns(Guid.NewGuid().ToString);

            ProductRepositoryMock = new Mock<IProductRepository>();
            Logger = new NullLogger<ProductsController>();

            return this;
        }

        public ProductsControllerTestFixture WithSetupForGet(Models.Products products)
        {
            ProductRepositoryMock.Setup(m => m.GetAsync()).Returns(Task.FromResult(products));

            return this;
        }

        public ProductsControllerTestFixture WithSetupForGet(Models.Product product)
        {
            ProductRepositoryMock.Setup(m => m.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult(product));

            return this;
        }



        public ProductsControllerTestFixture WithSetupForGet(BadRequestException exception)
        {
            ProductRepositoryMock.Setup(m => m.GetAsync(It.IsAny<Guid>())).Throws(exception);
            ProductRepositoryMock.Setup(m => m.GetAsync()).Throws(exception);

            return this;
        }

        public ProductsControllerTestFixture WithSetupForGetOptions(Models.ProductOptions options)
        {
            ProductRepositoryMock.Setup(m => m.GetOptionsAsync(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(options));

            return this;
        }

        public ProductsControllerTestFixture WithSetupForGetOptions(BadRequestException exception)
        {
            ProductRepositoryMock.Setup(m => m.GetOptionsAsync(It.IsAny<Guid>(), It.IsAny<string>())).Throws(exception);

            return this;
        }

        public ProductsControllerTestFixture WithSetupForGetOption(Models.ProductOption option)
        {
            ProductRepositoryMock.Setup(m => m.GetOptionAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(option));

            return this;
        }

        public ProductsControllerTestFixture WithSetupForGetOption(BadRequestException exception)
        {
            ProductRepositoryMock.Setup(m => m.GetOptionAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).Throws(exception);

            return this;
        }



        public ProductsControllerTestFixture WithNoProduct()
        {
            ProductRepositoryMock.Setup(m => m.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult((Models.Product)null));

            return this;
        }

        public ProductsControllerTestFixture WithSetupForNoOptions()
        {
            ProductRepositoryMock.Setup(m => m.GetOptionsAsync(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult((Models.ProductOptions)null));
            ProductRepositoryMock.Setup(m => m.GetOptionAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult((Models.ProductOption)null));

            return this;
        }

        public ProductsControllerTestFixture WithSetupForAdd(BadRequestException exception)
        {
            ProductRepositoryMock.Setup(m => m.AddAsync(It.IsAny<Product>(), It.IsAny<string>())).Throws(exception);
            ProductRepositoryMock.Setup(m => m.AddAsync(It.IsAny<Guid>(), It.IsAny<ProductOption>(), It.IsAny<string>())).Throws(exception);

            return this;
        }

        public ProductsControllerTestFixture WithSetupForUpdate(BadRequestException exception)
        {
            ProductRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Product>(), It.IsAny<string>())).Throws(exception);
            ProductRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ProductOption>(), It.IsAny<string>())).Throws(exception);

            return this;
        }

        public ProductsControllerTestFixture WithSetupForDelete(BadRequestException exception)
        {
            ProductRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>())).Throws(exception);
            ProductRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).Throws(exception);

            return this;
        }


        public ProductsController Build()
        {
            var sut = new ProductsController(ProductRepositoryMock.Object, Logger);
            sut.ControllerContext.HttpContext = HttpContextMock.Mock.Object;

            return sut;
        }

        public Models.Products GetProducts()
        {
            var p1 = _productBuilder.Start().Build();
            var p2 = _productBuilder.Start().Build();

            return new Models.Products(new[] { p1, p2 });
        }

        public Models.Product GetProduct()
        {
            return _productBuilder.Start().Build();
        }

        public Models.ProductOptions GetOptions()
        {
            var productId = Guid.NewGuid();

            var data = new[]
            {
                _optionBuilder.Start().WithProduct(productId).Build(),
                _optionBuilder.Start().WithProduct(productId).Build(),
                _optionBuilder.Start().WithProduct(productId).Build(),
                _optionBuilder.Start().WithProduct(productId).Build(),
            };

            return new ProductOptions(data);
        }

        public ProductOption GetOption()
        {
            return _optionBuilder.Start().Build();
        }
    }
}

