using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class Territory: ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedTerritory(context);
        }

        private static void SeedTerritory(IAdaniContext context)
        {
            context.Territory.AddOrUpdate(x => x.Id, new Entities.Territory
            {
                Id = 1,
                Name = "TNT",
                StateId = 1,
                IsActive = true,
            }
         );
        }
    }
}
