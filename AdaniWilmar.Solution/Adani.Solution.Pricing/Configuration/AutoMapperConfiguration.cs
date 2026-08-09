using AutoMapper;
using Adani.Solution.Data.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace Adani.Solution.Console.Configuration
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BaseSkuPriceDetails, Data.Entities.Pricing>();
                cfg.CreateMap<Data.Entities.Pricing, BaseSkuPriceDetails>();
            }, NullLoggerFactory.Instance);
            return config;
        }
    }
}
