using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Products.Api
{
    public class Startup_Tests
    {
        [Fact]
        public void Service_can_configure_services()
        {
            // arrange 
            var configMock = new Mock<IConfiguration>();

            var sut = new Startup(configMock.Object);

            // act & assert
            sut.ConfigureServices(new ServiceCollection());
            Assert.Equal(configMock.Object, sut.Configuration);
        }

        [Fact]
        public void Service_can_configure()
        {
            // arrange 
            var configMock = new Mock<IConfiguration>();
            var appBuilderMock = new Mock<IApplicationBuilder>();
            var envMock = new Mock<IWebHostEnvironment>();

            var sut = new Startup(configMock.Object);

            var serviceCollection = new ServiceCollection();

            sut.ConfigureServices(serviceCollection);
            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();

            var appBuilder = new ApplicationBuilder(serviceCollection.BuildServiceProvider());

            // act & assert
            sut.Configure(appBuilder, envMock.Object);

        }
    }
}