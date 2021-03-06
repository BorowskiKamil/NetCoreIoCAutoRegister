using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
	public class ServiceRegistration
	{

		private readonly IServiceCollection _serviceCollection;

		private ServiceLifetime _serviceLifetime = ServiceLifetime.Singleton;

		private Func<Type, bool> _typeRequirements; 

		public List<Assembly> Assemblies { get; set; } = new List<Assembly>();

		private bool _publicOnly = false;

		private List<Type> _exceptTypes { get; set; } = new List<Type>();


		public ServiceRegistration(IServiceCollection serviceCollection) 
		{
			_serviceCollection = serviceCollection;
		}

		public ServiceRegistration OfAssemblies(IEnumerable<Assembly> assemblies) 
		{
			Assemblies.AddRange(assemblies);
            return this;
        }

        public ServiceRegistration OfAssembly(Assembly assembly) 
		{
			return OfAssemblies(new List<Assembly> { assembly });
        }

		public ServiceRegistration Where(Func<Type, bool> predicate)
		{
			_typeRequirements = predicate;
			return this;
		}

		public ServiceRegistration PublicOnly()
		{
			_publicOnly = true;
			return this;
		}

		public ServiceRegistration Except<TType>()
		{
			_exceptTypes.Add(typeof(TType));
			return this;
		}

		public void AsSingleton()
		{
            _serviceLifetime = ServiceLifetime.Singleton;
            RegisterServices();
        }

        public void AsTransient()
		{
            _serviceLifetime = ServiceLifetime.Transient;
            RegisterServices();
        }

        public void AsScoped()
		{
            _serviceLifetime = ServiceLifetime.Scoped;
            RegisterServices();
        }

		private void RegisterServices()
		{
			var interfaces = Assemblies.SelectMany(i => i.GetTypes())
					.SelectMany(i => i.GetInterfaces())
					.Where(_typeRequirements)
					.Except(_exceptTypes);

			if (_publicOnly)
			{
				interfaces = interfaces.Where(i => i.IsPublic);
			}

			foreach (var interfaceType in interfaces) 
			{
                if (_serviceCollection.Any(s => s.ServiceType == interfaceType)) continue;

                var implementation = Assemblies.SelectMany(p => p.GetTypes())
                                     .FirstOrDefault(t => interfaceType.IsAssignableFrom(t)
                                                          && !t.IsInterface 
                                                          && !t.IsAbstract);
				if (implementation == null) continue;
				
				_serviceCollection.Add(new ServiceDescriptor(interfaceType, implementation, _serviceLifetime));
            }
		}

	}
}