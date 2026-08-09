using System.Data.Entity.Migrations;
using Adani.Solution.Data.DatabaseContext;

namespace Adani.Solution.Data.Seeder
{
    public class MTPStatus : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedMTPStatus(context);
        }
        private static void SeedMTPStatus(IAdaniContext context)
        {
            context.MonthlyTourPlanStatus.AddOrUpdate(x => x.Id,
                new Entities.MonthlyTourPlanStatus
                {
                    Id = 1,
                    Status = "Pending"
                },
                new Entities.MonthlyTourPlanStatus
                {
                    Id = 2,
                    Status = "Approved"
                },
                new Entities.MonthlyTourPlanStatus
                {
                    Id = 3,
                    Status = "Rejected"
                },
                new Entities.MonthlyTourPlanStatus
                {
                    Id = 4,
                    Status = "Drafted"
                }
            );
        }
    }
}
