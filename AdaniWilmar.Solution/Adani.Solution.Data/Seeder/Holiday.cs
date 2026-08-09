using Adani.Solution.Data.DatabaseContext;
using System;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class Holiday : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedHoliday(context);
        }

        private static void SeedHoliday(IAdaniContext context)
        {
            context.Holiday.AddOrUpdate(a => a.Id,
                new Entities.Holiday()
                {
                    Id = 1,
                    HolidayName = "Diwali",
                    HolidayDate = new DateTime(2018, 11, 7),
                    Description = "Deepavali",
                    Year = 2018
                },
                new Entities.Holiday()
                {
                    Id = 2,
                    HolidayName = "Christmas Day",
                    HolidayDate = new DateTime(2018, 12, 25),
                    Description = "Christmas Day",
                    Year = 2018
                },
                new Entities.Holiday()
                {
                    Id = 3,
                    HolidayName = "Dussehra",
                    HolidayDate = new DateTime(2018, 10, 19),
                    Description = "Dussehra",
                    Year = 2018
                },
                new Entities.Holiday()
                {
                    Id = 4,
                    HolidayName = "Gandhi Jayanti",
                    HolidayDate = new DateTime(2018, 10, 2),
                    Description = "Gandhi Jayanti",
                    Year = 2018
                },
                new Entities.Holiday()
                {
                    Id = 5,
                    HolidayName = "Guru Nanak Jayanti",
                    HolidayDate = new DateTime(2018, 11, 23),
                    Description = "Guru Nanak Jayanti",
                    Year = 2018
                });
        }
    }
}
