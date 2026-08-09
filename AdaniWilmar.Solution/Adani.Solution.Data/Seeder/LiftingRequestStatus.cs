using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class LiftingRequestStatus : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedLiftingRequestStatus(context);
        }

        private static void SeedLiftingRequestStatus(IAdaniContext context)
        {
            context.LiftingRequestStatus.AddOrUpdate(x => x.Id, new Entities.LiftingRequestStatus
            {
                Id = 1,
                Name = "Inprogress",
                IsActive = true,
            },
            new Entities.LiftingRequestStatus
            {
                Id = 2,
                Name = "Confirmed",
                IsActive = true,
            },
            new Entities.LiftingRequestStatus
            {
                Id = 3,
                Name = "Intransist",
                IsActive = true,
            });
        }
    }
}
