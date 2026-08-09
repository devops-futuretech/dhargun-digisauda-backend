using Adani.Solution.Data.DatabaseContext;
using System;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class PackGroup : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedOilPackingType(context);
        }

        private static void SeedOilPackingType(IAdaniContext context)
        {
            context.OilPackingTypes.AddOrUpdate(x => x.Id, new Entities.PackGroup
            {
                Id = Convert.ToInt64(DTO.Enums.PackGroupType.Premium),
                Name = DTO.Enums.PackGroupType.Premium.ToString(),
                IsActive = true,
                SAPName= DTO.Enums.PackGroupType.Premium.ToString(),
            },
            new Entities.PackGroup
            {
                Id = Convert.ToInt64(DTO.Enums.PackGroupType.Bakery),
                Name = DTO.Enums.PackGroupType.Bakery.ToString(),
                IsActive = true,
                SAPName = DTO.Enums.PackGroupType.Bakery.ToString(),
            },
            new Entities.PackGroup
            {
                Id = Convert.ToInt64(DTO.Enums.PackGroupType.Popular),
                Name = DTO.Enums.PackGroupType.Popular.ToString(),
                IsActive = true,
                SAPName= DTO.Enums.PackGroupType.Popular.ToString(),
            },
            new Entities.PackGroup
            {
                Id = Convert.ToInt64(DTO.Enums.PackGroupType.Lauric),
                Name = DTO.Enums.PackGroupType.Lauric.ToString(),
                IsActive = true,
                SAPName= DTO.Enums.PackGroupType.Lauric.ToString(),
            });
        }
    }
}
