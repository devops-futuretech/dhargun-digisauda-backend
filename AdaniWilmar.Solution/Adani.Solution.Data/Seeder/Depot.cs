using System;
using System.Collections.Generic;
using System.Linq;
using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{

    public class Depot : ISeeder
    {
        public void Seed(AdaniContext context)
        {
           //SeedDepot(context);
        }
        private static void SeedDepot(IAdaniContext context)
        {
            context.Depots.AddOrUpdate(x => x.Id,
                 new Entities.Depot
                 {
                     Id = 1,
                     Name = "Depot1",
                     Code = "Depot1",
                     //StateId = 1,
                     ////TerritoryId = 1,
                     //DistrictId = 1,
                     //CityId = 1
                 },
                 new Entities.Depot
                 {
                     Id = 2,
                     Name = "Depot2",
                     Code = "Depot2",
                     //StateId = 1,
                     ////TerritoryId = 1,
                     //DistrictId = 1,
                     //CityId = 1
                 }
             );
        }
    }
}
