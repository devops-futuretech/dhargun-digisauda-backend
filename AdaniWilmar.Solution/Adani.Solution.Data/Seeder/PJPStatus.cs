using System.Data.Entity.Migrations;
using Adani.Solution.Data.DatabaseContext;

namespace Adani.Solution.Data.Seeder
{
    class PJPStatus : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedPJPStatus(context);
        }
        private static void SeedPJPStatus(AdaniContext context)
        {
            context.PJPStatus.AddOrUpdate(x => x.Id,
                new Entities.PermanentJourneyPlanStatus
                {
                    Id = 1,
                    Status = "Pending"
                },
                new Entities.PermanentJourneyPlanStatus
                {
                    Id = 2,
                    Status = "Approved"
                },
                new Entities.PermanentJourneyPlanStatus
                {
                    Id = 3,
                    Status = "Rejected"
                },
                new Entities.PermanentJourneyPlanStatus
                {
                    Id = 4,
                    Status = "Drafted"
                }
            );
        }
    }
}
