using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class ZoneStateMapping : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedZoneStateMapping(context);
        }
        public void SeedZoneStateMapping(AdaniContext context)
        {
            context.ZoneStateMappings.AddOrUpdate(x => x.Id,
             new Entities.ZoneStateMapping()
             {
                 Id = 1,
                 ZoneId = 1,
                 StateId = 1
             }
             );
        }
    }
}


