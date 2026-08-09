using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;
using Castle.Core;
using Castle.MicroKernel.Registration;

namespace Adani.Solution.MVC.Infrastructure
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
            //_kernel.Register(Component.For(typeof(CaptchaMvc.Controllers.DefaultCaptchaController)).Named("ExternalResources").LifeStyle.Is(LifestyleType.Transient));
        }

        public override void ReleaseController(IController controller)
        {
            _kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                //throw new HttpException(404,
                //    $"The controller for path '{requestContext.HttpContext.Request.Path}' could not be found.");
                return null;
            }
            return (IController)_kernel.Resolve(controllerType);
        }
    }
}