using System;
using System.Data.Entity.Migrations;
using Adani.Solution.Data.DatabaseContext;

namespace Adani.Solution.Data.Seeder
{
    public class BiddingWindowStatus : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedBiddingWindowStatus(context);
        }

        private static void SeedBiddingWindowStatus(IAdaniContext context)
        {
            context.BiddingWindowStatus.AddOrUpdate(x => x.Id, new Entities.BiddingWindowStatus
            {
                Id = Convert.ToInt64(DTO.Enums.BiddWindowStatus.Pending),
                Name = DTO.Enums.BiddWindowStatus.Pending.ToString(),
                IsActive = true,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow
            },
            new Entities.BiddingWindowStatus
            {
                Id = Convert.ToInt64(DTO.Enums.BiddWindowStatus.Processing),
                Name = DTO.Enums.BiddWindowStatus.Processing.ToString(),
                IsActive = true,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow
            }, new Entities.BiddingWindowStatus
            {
                Id = Convert.ToInt64(DTO.Enums.BiddWindowStatus.Stopped),
                Name = DTO.Enums.BiddWindowStatus.Stopped.ToString(),
                IsActive = true,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow
            }, new Entities.BiddingWindowStatus
            {
                Id = Convert.ToInt64(DTO.Enums.BiddWindowStatus.Completed),
                Name = DTO.Enums.BiddWindowStatus.Completed.ToString(),
                IsActive = true,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow
            });
        }
    }
}
