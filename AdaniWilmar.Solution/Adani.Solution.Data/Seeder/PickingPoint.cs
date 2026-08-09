using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;
namespace Adani.Solution.Data.Seeder
{
  
    public class PickingPoint : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedPickingPoint(context);
        }

        private static void SeedPickingPoint(IAdaniContext context)
        {
            context.PickingPoints.AddOrUpdate(x => x.Id, new Entities.PickingPoint
            {
                Id = 1,
                Name = "Good Stock - FG",
                Code = "130",
                IsActive = true,
            },
            new Entities.PickingPoint
            {
                Id = 2,
                Name = "Modern Trade",
                Code = "140",
                IsActive = true,
            });
        }
    }
}
