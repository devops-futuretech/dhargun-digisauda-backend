using AutoMapper;
using Adani.Solution.Console.Configuration;
using SimpleInjector;

namespace Adani.Solution.Console.Common
{
    public static class SimpleInjectorConfguration
    {
        public static Container Initialize()
        {
            //1. Create instance of container
            Container container = new Container();

            //2. Configure the container
            var mapper = AutoMapperConfiguration.InitializeAutoMapper().CreateMapper();
            container.RegisterInstance<IMapper>(mapper);

            //3. Verify the container
            container.Verify();
            return container;
        }
    }
}
