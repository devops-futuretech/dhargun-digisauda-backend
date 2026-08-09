using AutoMapper;
using Adani.Solution.Console.Configuration;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Console
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
