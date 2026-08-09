using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class SaudaBookingType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedSaudaBookingType(context);
        }

        private static void SeedSaudaBookingType(IAdaniContext context)
        {
            context.SaudaBookingTypes.AddOrUpdate(x => x.Id, new Entities.SaudaBookingType
            {
                Id = 1,
                Name = "Traditional process",
                IsActive = true,
            }
            //,
            //new Entities.SaudaBookingType
            //{
            //    Id = 2,
            //    Name = "Reverse Auction",
            //    IsActive = true,
            //}
            );
        }
    }
}
