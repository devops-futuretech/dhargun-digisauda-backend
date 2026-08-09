using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class MaterialType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedMaterialType(context);
        }
        public void SeedMaterialType(AdaniContext context)
        {
            //context.MaterialTypes.AddOrUpdate(x => x.Id,
            // new Entities.MaterialType() { 
            //     Id = 1, 
            //     Name = "SFO",
            //     IsActive = true,
            //     SalesOrganizationId=1,
            //     DistributionChannelId=1,
            //     DivisionId=1 }
            // );
        }
    }
}


