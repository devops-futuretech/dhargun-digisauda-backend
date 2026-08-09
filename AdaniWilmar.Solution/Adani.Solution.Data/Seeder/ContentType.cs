using Adani.Solution.Data.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Seeder
{
    public class ContentType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedContentType(context);
        }

        private static void SeedContentType(IAdaniContext context)
        {
            context.ContentType.AddOrUpdate(x => x.Id, new Entities.ContentType
            {
                Id = 1,
                Name = "LatestUpdate",
                IsActive = true,
            },
            new Entities.ContentType
            {
                Id = 2,
                Name = "SpecialInformation",
                IsActive = true,
            }
            );
        }
    }
}
