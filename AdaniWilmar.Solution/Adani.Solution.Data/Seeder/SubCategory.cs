using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
  public class SubCategory : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedSubCategory(context);
        }

        private static void SeedSubCategory(IAdaniContext context)
        {
            context.SubCategory.AddOrUpdate(x => x.Id, new Entities.SubCategory
            {
                Id = 1,
                Name = "Gold",
                IsActive = true,
            },
            new Entities.SubCategory
            {
                Id = 2,
                Name = "Blue",
                IsActive = true,
            },
            new Entities.SubCategory
            {
                Id = 3,
                Name = "Yellow",
                IsActive = true,
            });
        }
    }
}
