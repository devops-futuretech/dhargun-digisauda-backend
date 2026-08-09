using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class District : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedDistrict(context);
        }

        private static void SeedDistrict(IAdaniContext context)
        {
            context.District.AddOrUpdate(x => x.Id, new Entities.District
            {
                Id = 1,
                DistrictName ="Coimbatore",
                StateId = 1,
                //TerritoryId = 1,
                IsActive = true,
            }
           );
        }
    }
}
