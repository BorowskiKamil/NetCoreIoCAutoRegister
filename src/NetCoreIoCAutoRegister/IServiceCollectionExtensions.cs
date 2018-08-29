using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {

        public static ServiceRegistration RegisterTypes(this IServiceCollection serviceCollection) 
        {
            var registration = new ServiceRegistration(serviceCollection);
            return registration;
        }

    }
}
