using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class Country : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedCountry(context);
        }

        private static void SeedCountry(IAdaniContext context)
        {
            context.Country.AddOrUpdate(x => x.Id, new Entities.Country
            {
                Id = 1,
                Name = "India",
                Code = "1",
                SortOrder = 1,
                IsActive = true,
            });
        }
    }
}
