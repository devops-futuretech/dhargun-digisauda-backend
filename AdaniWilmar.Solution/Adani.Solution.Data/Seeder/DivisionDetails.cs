using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    class DivisionDetails : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedDivisionDetails(context);
        }

        private static void SeedDivisionDetails(IAdaniContext context)
        {
            //context.DivisionDetails.AddOrUpdate(x => x.Id, new Entities.DivisionDetail
            //{
            //    Id = 1,
            //   CCArea= "2000",
            //   DivisionId=1
            //},
            //new Entities.DivisionDetail
            //{
            //    Id = 2,
            //    CCArea = "3000",
            //    DivisionId = 2

            //}
            //,new Entities.DivisionDetail
            //{
            //    Id = 3,
            //    CCArea = "1000",
            //    DivisionId = 3
            //},
            //new Entities.DivisionDetail
            //{
            //    Id = 4,
            //    CCArea = "7000",
            //    DivisionId = 3
            //}
            //);
        }
    }
}