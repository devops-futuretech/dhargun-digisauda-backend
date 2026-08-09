using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class DeliveryPriority : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedDeliveryPriority(context);
        }

        private static void SeedDeliveryPriority(IAdaniContext context)
        {
            context.DeliveryPriorities.AddOrUpdate(x => x.Id, new Entities.DeliveryPriority
            {
                Id = 1,
                Name = "High",
                Code = "01",
                IsActive = true,
            },
            new Entities.DeliveryPriority
            {
                Id = 2,
                Name = "Normal",
                Code = "02",
                IsActive = true,
            });
        }
    }
}
