using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;
namespace Adani.Solution.Data.Seeder
{
    public class City : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedCity(context);
        }

        private static void SeedCity(IAdaniContext context)
        {
            context.City.AddOrUpdate(x => x.Id, new Entities.City
            {
                Id = 1,
                CityName = "Coimbatore North",
                DistrictId = 1,
                //TerritoryId = 1,
                IsActive = true,
            });
        }
    }
}
