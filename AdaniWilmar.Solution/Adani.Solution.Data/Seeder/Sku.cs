using Adani.Solution.Data.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Seeder
{
    public class Sku : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedSku(context);
        }
        private static void SeedSku(IAdaniContext context)
        {
            context.Skus.AddOrUpdate(x => x.Id,
                 new Entities.Sku
                 {
                     Id = 1,
                     SkuCode = "Prod Code",
                     SkuName = "Prod Name",
                     //DepotId = 1,
                     OilTypeId =1,
                     PackGroupId =1,
                     PackTypeId =1,
                     UomId =1,
                     IsActive = true,
                     DivisionId =1,
                     IsSAPData = true,
                     IsSAPDataSyncOrNot = true
                 }
             );
        }
    }
}
