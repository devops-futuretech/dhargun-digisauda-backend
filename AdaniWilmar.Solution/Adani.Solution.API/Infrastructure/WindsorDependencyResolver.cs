using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace Adani.Solution.API.Infrastructure
{
    public class WindsorDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer _container;
        private readonly ILogger _logger = Logging.GetLogger("API WindsorDependencyResolver");

        public WindsorDependencyResolver(IWindsorContainer container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            try
            {
                return new WindsorDependencyScope(_container);
            }
            catch (Exception exception)
            {
                _logger.Fatal("API BeginScope Error:", exception);
            }
            return null;
        }

        public object GetService(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType) ? _container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            // This "not has component" part is from "Pro ASP.NET Web API" p. 426
            if (!_container.Kernel.HasComponent(serviceType))
            {
                return new object[0];
            }

            return _container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
    public class WindsorDependencyScope : IDependencyScope
    {
        private readonly IWindsorContainer _container;
        private readonly IDisposable _scope;
        private readonly ILogger _logger = Logging.GetLogger("API WindsorDependencyScope Error");

        public WindsorDependencyScope(IWindsorContainer container)
        {
            _container = container;
            _scope = container.BeginScope();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                if (_container.Kernel.HasComponent(serviceType))
                {
                    return _container.Resolve(serviceType);
                }
                return null;
            }
            catch (Exception exception)
            {
                _logger.Fatal("API Kernel Resolve Error:", exception);
            }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}