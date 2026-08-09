using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;


namespace Adani.Solution.Data.Seeder
{
    public class Division : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedDivision(context);
        }

        private static void SeedDivision(IAdaniContext context)
        {
            context.Divisions.AddOrUpdate(x => x.Id, new Entities.Division
            {
                Id = 1,
                Name = "Oil traded",
                Code = "12",
                SalesOrganizationId = 1,
                DistributionChannelId = 1,
                SalesDocumentType = "ZCOL",
                IsActive = true,
            },
            new Entities.Division
            {
                Id = 2,
                Name = "Bakery",
                Code = "26",
                SalesOrganizationId = 1,
                DistributionChannelId = 1,
                SalesDocumentType = "ZCOL",
                IsActive = true,
            },
            new Entities.Division
            {
                Id = 3,
                Name = "Lauric",
                Code = "28",
                SalesOrganizationId = 1,
                DistributionChannelId = 1,
                SalesDocumentType = "ZCOL",
                IsActive = true,
            },
            new Entities.Division
            {
                Id = 4,
                Name = "Oil traded",
                Code = "12",
                SalesOrganizationId = 1,
                DistributionChannelId = 2,
                SalesDocumentType = "ZCOL",
                IsActive = true,
            }
            );
        }
    }
}
