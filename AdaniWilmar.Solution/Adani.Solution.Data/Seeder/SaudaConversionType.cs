using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO.Enums;
using System;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class SaudaConversionType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedConfiguration(context);
        }
        private static void SeedConfiguration(IAdaniContext context)
        {
            context.saudaConversionTypes.AddOrUpdate(x => x.Id,
                new Entities.SaudaConversionType
                {
                    Id = 1,
                    Name = "CP to CP",
                    IsActive = false,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now
                }, new Entities.SaudaConversionType
                {
                    Id = 2,
                    Name = "BP to BP",
                    IsActive = false,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now
                }, new Entities.SaudaConversionType
                {
                    Id = 3,
                    Name = "BP to CP",
                    IsActive = false,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now
                }, new Entities.SaudaConversionType
                {
                    Id = 4,
                    Name = "CP to BP",
                    IsActive = false,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now
                });
        }
    }
}
