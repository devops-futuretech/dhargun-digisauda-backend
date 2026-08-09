using System.Data.Entity.Migrations;
using Adani.Solution.Data.DatabaseContext;

namespace Adani.Solution.Data.Seeder
{
    public class MonthlyPlanDeviation : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedMonthlyPlanDeviationStatus(context);
        }
        private static void SeedMonthlyPlanDeviationStatus(IAdaniContext context)
        {
            context.MonthlyPlanDeviationStatus.AddOrUpdate(x => x.Id,
                new Entities.MonthlyPlanDeviationStatus
                {
                    Id = 1,
                    Status = "Pending"
                },
                new Entities.MonthlyPlanDeviationStatus
                {
                    Id = 2,
                    Status = "Approved"
                },
                new Entities.MonthlyPlanDeviationStatus
                {
                    Id = 3,
                    Status = "Rejected"
                }
            );
        }
    }
}
