using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NetCoreIoCAutoRegister.UnitTests.Helpers;
using Xunit;
using System.Linq;

namespace NetCoreIoCAutoRegister.UnitTests
{
    public class ServiceCollectionTests
    {

        private IServiceCollection _serviceCollection;

        public ServiceCollectionTests() 
        {
            _serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void Should_Register_Three_Services()
        {
            _serviceCollection.RegisterTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Repository"))
                .Except<IFourthRepository>()
                .AsScoped();

            Assert.Equal(3, _serviceCollection.Count());
        }

        [Fact]
        public void Should_Register_Specified_Service()
        {
            _serviceCollection.RegisterTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Repository"))
                .AsSingleton();

            var serviceProvider = _serviceCollection.BuildServiceProvider();

            Assert.IsType<FirstRepository>(serviceProvider.GetService<IFirstRepository>());
        }
    }
}
