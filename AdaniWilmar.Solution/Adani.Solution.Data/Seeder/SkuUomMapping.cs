using Adani.Solution.Data.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Seeder
{
    public class SkuUomMapping : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedSkuUomMapping(context);
        }
        private static void SeedSkuUomMapping(IAdaniContext context)
        {
            //context.SkuUomMapping.AddOrUpdate(x => x.Id,
            //     new Entities.SkuUomMapping
            //     {
            //        SkuId = 1,
            //        UomId = 1,
            //        RelationUomId =5,
            //        ConversionFactor =1
            //     },
            //       new Entities.SkuUomMapping
            //       {
            //           SkuId = 1,
            //           UomId = 3,
            //           RelationUomId = 2,
            //           ConversionFactor = 1
            //       }
            // );
        }
    }
}
