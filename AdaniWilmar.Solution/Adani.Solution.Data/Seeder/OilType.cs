using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class OilType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedOilType(context);
        }

        private static void SeedOilType(IAdaniContext context)
        {
            context.OilTypes.AddOrUpdate(x => x.Id, new Entities.OilType
            {
                Id = 1,
                Name = "Palm",
                SalesOrganizationId = 1,
                DistributionChannelId = 1,
                DivisionId = 1,
                //LitreConversion = (decimal)1098.90,
                IsActive = true,
                SAPCode="10"
            });
        }
    }
}
