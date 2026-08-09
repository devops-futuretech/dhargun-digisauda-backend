using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class DistributionChannel : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedDistributionChannel(context);
        }

        private static void SeedDistributionChannel(IAdaniContext context)
        {
            context.DistributionChannel.AddOrUpdate(x => x.Id, new Entities.DistributionChannel
            {
                Id = 1,
                Name = "Customer sale",
                SalesOrganizationId = 1,
                IsActive = true,
                Code = "10"
            },
            //new Entities.DistributionChannel
            //{
            //    Id = 3,
            //    Name = "15",
            //    IsActive = true,
            //    SalesOrganizationId = 1,
            //    SAPCode = "15"

            //},
            //new Entities.DistributionChannel
            //{
            //    Id = 4,
            //    Name = "20",
            //    IsActive = true,
            //    SalesOrganizationId = 1,
            //    SAPCode = "20"
            //},
            //new Entities.DistributionChannel
            //{
            //    Id = 5,
            //    Name = "25",
            //    IsActive = true,
            //    SalesOrganizationId = 1,
            //    SAPCode = "25"
            //},
            //new Entities.DistributionChannel
            //{
            //    Id = 6,
            //    Name = "35",
            //    IsActive = true,
            //    SalesOrganizationId = 1,
            //    SAPCode = "35"
            //},
            //new Entities.DistributionChannel
            //{
            //    Id = 7,
            //    Name = "40",
            //    IsActive = true,
            //    SalesOrganizationId = 1,
            //    SAPCode = "40"
            //},
            //new Entities.DistributionChannel
            //{
            //    Id = 8,
            //    Name = "45",
            //    IsActive = true,
            //    SalesOrganizationId = 1,
            //    SAPCode = "45"
            //},
            //new Entities.DistributionChannel
            //{
            //    Id = 9,
            //    Name = "40",
            //    IsActive = true,
            //    SalesOrganizationId = 1,
            //    SAPCode = "40"
            //},
            //new Entities.DistributionChannel
            //{
            //    Id = 10,
            //    Name = "50",
            //    IsActive = true,
            //    SalesOrganizationId = 1,
            //    SAPCode = "50"
            //},
            //new Entities.DistributionChannel
            //{
            //    Id = 11,
            //    Name = "60",
            //    IsActive = true,
            //    SalesOrganizationId = 1,
            //    SAPCode = "60"
            //},
            new Entities.DistributionChannel
            {
                Id = 2,
                Name = "Popular",
                IsActive = true,
                SalesOrganizationId = 1,
                Code = "75"
            }
            );
        }
    }
}
