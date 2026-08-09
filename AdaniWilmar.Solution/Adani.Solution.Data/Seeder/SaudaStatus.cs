using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;
namespace Adani.Solution.Data.Seeder
{
    public class SaudaStatus : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedSaudaStatus(context);
        }

        private static void SeedSaudaStatus(IAdaniContext context)
        {
            context.SaudaStatus.AddOrUpdate(x => x.Id, new Entities.SaudaStatus
            {
                Id = 1,
                Name = "Not Released",
                IsActive = true,
            },
            new Entities.SaudaStatus
            {
                Id = 2,
                Name = "Released",
                IsActive = true,
            },
            new Entities.SaudaStatus
            {
                Id = 3,
                Name = "Open",
                IsActive = true,
            }, new Entities.SaudaStatus
            {
                Id = 4,
                Name = "Blocked",
                IsActive = true,
            }, new Entities.SaudaStatus
            {
                Id = 5,
                Name = "Processed",
                IsActive = true,
            });
        }
    }
}
