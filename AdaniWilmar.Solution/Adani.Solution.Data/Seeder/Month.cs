using GMCore.Helper;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO.Enums;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class Month : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedMonth(context);
        }
        private static void SeedMonth(IAdaniContext context)
        {
            context.Months.AddOrUpdate(x => x.Id,
                new Entities.Month
                {
                    Id = 1,
                    Name = "January"
                },
                new Entities.Month
                {
                    Id = 2,
                    Name = "February"
                },
                new Entities.Month
                {
                    Id = 3,
                    Name = "March"
                },
                new Entities.Month
                {
                    Id = 4,
                    Name = "April"
                },
                new Entities.Month
                {
                    Id = 5,
                    Name = "May"
                },
                new Entities.Month
                {
                    Id = 6,
                    Name = "June"
                },
                new Entities.Month
                {
                    Id = 7,
                    Name = "July"
                },
                new Entities.Month
                {
                    Id = 8,
                    Name = "August"
                },
                new Entities.Month
                {
                    Id = 9,
                    Name = "September"
                },
                new Entities.Month
                {
                    Id = 10,
                    Name = "October"
                },
                new Entities.Month
                {
                    Id = 11,
                    Name = "November"
                },
                new Entities.Month
                {
                    Id = 12,
                    Name = "December"
                }
            );
        }
    }
}
