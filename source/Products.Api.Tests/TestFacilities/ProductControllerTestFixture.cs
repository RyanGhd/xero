using System;
using System.Threading.Tasks;
using HttpContextMoq;
using Microsoft.AspNetCore.Http;
using Moq;
using Products.Api.Controllers;
using Products.Api.Data;
using Products.Api.Models;
using Products.Api.Models.Exceptions;

namespace Products.Api.TestFacilities
{
    public class ProductsControllerTestFixture
    {
        private readonly ProductDataBuilder _productDtaBuilder = new ProductDataBuilder();

        private HttpContextMock HttpContextMock { get; set; }
        private Mock<IProductRepository> ProductRepositoryMock { get; set; }

        public ProductsControllerTestFixture Start()
        {
            HttpContextMock = new HttpContextMock();
            HttpContextMock.Mock.Setup(c => c.TraceIdentifier).Returns(Guid.NewGuid().ToString);

            ProductRepositoryMock = new Mock<IProductRepository>();


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

        public ProductsControllerTestFixture WithNoProduct()
        {
            ProductRepositoryMock.Setup(m => m.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult((Models.Product)null));

            return this;
        }

        public ProductsControllerTestFixture WithSetupForAdd(BadRequestException exception)
        {
            ProductRepositoryMock.Setup(m => m.AddAsync(It.IsAny<Product>(), It.IsAny<string>())).Throws(exception);

            return this;
        }
        

        public ProductsController Build()
        {
            var sut = new ProductsController(ProductRepositoryMock.Object);
            sut.ControllerContext.HttpContext = HttpContextMock.Mock.Object;

            return sut;
        }

        public Models.Products GetProducts()
        {
            var p1 = _productDtaBuilder.Start().Build();
            var p2 = _productDtaBuilder.Start().Build();

            return new Models.Products(new[] { p1, p2 });
        }

        public Models.Product GetProduct()
        {
            return _productDtaBuilder.Start().Build();
        }
    }
}

