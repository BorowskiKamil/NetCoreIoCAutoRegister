using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {

        public static ServiceRegistration RegisterTypes(this IServiceCollection serviceCollection, Assembly assembly = null) 
        {
            var registration = new ServiceRegistration(serviceCollection);

            if (assembly != null) registration.Assemblies.Add(assembly);

            return registration;
        }

    }
}
