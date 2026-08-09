using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
   public class SalesOrganization : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedSalesOrganization(context);
        }

        private static void SeedSalesOrganization(IAdaniContext context)
        {
            context.SalesOrganization.AddOrUpdate(x => x.Id, new Entities.SalesOrganization
            {
                Id = 1,
                Name = "AWL Marketing",
                IsActive = true,
                Code = "1000"
            }
            );
        }
    }
}
