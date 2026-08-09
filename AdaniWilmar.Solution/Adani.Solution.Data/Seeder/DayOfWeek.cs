using GMCore.Helper;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO.Enums;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class DayOfWeek : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedDayOfWeekNames(context);
        }
        private static void SeedDayOfWeekNames(IAdaniContext context)
        {
            context.DayOfWeekNames.AddOrUpdate(x => x.Id,
                new Entities.DayOfWeekName
                {
                    Id = (int)DayOfWeekName.Monday,
                    Name = UtilityHelper.GetEnumDescription(DayOfWeekName.Monday),
                    IsHoliday = false,
                    SortOrder = 1
                },
                new Entities.DayOfWeekName
                {
                    Id = (int)DayOfWeekName.Tuesday,
                    Name = UtilityHelper.GetEnumDescription(DayOfWeekName.Tuesday),
                    IsHoliday = false,
                    SortOrder = 2
                },
                new Entities.DayOfWeekName
                {
                    Id = (int)DayOfWeekName.Wednesday,
                    Name = UtilityHelper.GetEnumDescription(DayOfWeekName.Wednesday),
                    IsHoliday = false,
                    SortOrder = 3
                },
                new Entities.DayOfWeekName
                {
                    Id = (int)DayOfWeekName.Thursday,
                    Name = UtilityHelper.GetEnumDescription(DayOfWeekName.Thursday),
                    IsHoliday = false,
                    SortOrder = 4
                },
                 new Entities.DayOfWeekName
                 {
                     Id = (int)DayOfWeekName.Friday,
                     Name = UtilityHelper.GetEnumDescription(DayOfWeekName.Friday),
                     IsHoliday = false,
                     SortOrder = 5
                 },
                 new Entities.DayOfWeekName
                 {
                     Id = (int)DayOfWeekName.Saturday,
                     Name = UtilityHelper.GetEnumDescription(DayOfWeekName.Saturday),
                     IsHoliday = false,
                     SortOrder = 6
                 },
                 new Entities.DayOfWeekName
                 {
                     Id = (int)DayOfWeekName.Sunday,
                     Name = UtilityHelper.GetEnumDescription(DayOfWeekName.Sunday),
                     IsHoliday = true,
                     SortOrder = 7
                 }
                );
        }
    }
}
