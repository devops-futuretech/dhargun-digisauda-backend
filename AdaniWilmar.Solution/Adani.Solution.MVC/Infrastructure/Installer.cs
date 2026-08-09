using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web.Mvc;
using GMCore.Logger;
using System.Web.Http;

namespace Adani.Solution.MVC.Infrastructure
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(FindControllers().Configure(ConfigureControllers));
            container.Register(FindApiControllers().Configure(ConfigureApiControllers));

            container.Register(Component.For<ILogger>().ImplementedBy<Logging>());
        }

        private BasedOnDescriptor FindControllers()
        {
            return Types.FromThisAssembly()
                   .BasedOn<IController>()
                   .If(t => t.Name.EndsWith("Controller"));
        }

        private void ConfigureControllers(ComponentRegistration registration)
        {
            registration.LifestyleTransient();
        }

        private BasedOnDescriptor FindApiControllers()
        {
            return Classes.FromThisAssembly().BasedOn<ApiController>();
        }

        private void ConfigureApiControllers(ComponentRegistration registration)
        {
            registration.LifestylePerWebRequest();
        }
    }
}