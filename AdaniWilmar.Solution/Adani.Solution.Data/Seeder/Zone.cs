using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using System.Data.Entity.Migrations;
namespace Adani.Solution.Data.Seeder
{
   public class Zone : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedZone(context);
        }
        public void SeedZone(AdaniContext context)
        {
            context.Zones.AddOrUpdate(x => x.Id,
             new Entities.Zone()
             {
                 Id = 2,
                 Name = "South",
                 IsActive = true,
             },
             new Entities.Zone()
             {
                 Id = 2,
                 Name = "East",
                 IsActive = true,
             },
             new Entities.Zone()
             {
                 Id = 3,
                 Name = "Central",
                 IsActive = true,
             },
             new Entities.Zone()
             {
                 Id = 4,
                 Name = "North",
                 IsActive = true,
             },
             new Entities.Zone()
             {
                 Id = 5,
                 Name = "West",
                 IsActive = true,
             }
             );
        }
    }
}
