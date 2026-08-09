using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class Status : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedApprovalStatus(context);
        }
        private static void SeedApprovalStatus(IAdaniContext context)
        {
            context.ApprovalStatus.AddOrUpdate(x => x.Id, new Entities.Status
            {
                Id = 1,
                Name = "Pending",
                IsActive = true,
            },
            new Entities.Status
            {
                Id = 2,
                Name = "Approved",
                IsActive = true,
            },
            new Entities.Status
            {
                Id = 3,
                Name = "Rejected",
                IsActive = true,
            },
            new Entities.Status
            {
                Id = 4,
                Name = "Revised",
                IsActive = true,
            },
            new Entities.Status
            {
                Id = 5,
                Name = "Hold",
                IsActive = true,
            },
            new Entities.Status
            {
                Id = 6,
                Name = "Completed",
                IsActive = true,
            },
             new Entities.Status
             {
                 Id = 7,
                 Name = "Waiting For Approval",
                 IsActive = true,
             },
             new Entities.Status
             {
                 Id = 8,
                 Name = "Processed",
                 IsActive = true,
             },
             new Entities.Status
             {
                 Id = 9,
                 Name = "Request For Approval",
                 IsActive = true,
             },
             new Entities.Status
             {
                 Id = 10,
                 Name = "Request For Approval2",
                 IsActive = true,
             },
             new Entities.Status
             {
                 Id = 11,
                 Name = "Waiting For Confirmation",
                 IsActive = true,
             },
             new Entities.Status
             {
                 Id = 12,
                 Name = "Requested",
                 IsActive = true,
             },
             new Entities.Status
             {
                 Id = 13,
                 Name = "Inprogress",
                 IsActive = true,
             },
             new Entities.Status
             {
                 Id = 14,
                 Name = "Deleted",
                 IsActive = true,
             });
        }
    }
}