using Products.Api.Controllers;

namespace Products.Api.TestFacilities
{
    public class ProductsControllerTestFixture
    {
        public ProductsControllerTestFixture Start()
        {
            return this;
        }

        public ProductsController Build()
        {
            return new ProductsController();
        }
    }
}

      