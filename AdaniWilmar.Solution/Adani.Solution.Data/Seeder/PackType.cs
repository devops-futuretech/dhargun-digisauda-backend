using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class PackType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedPackType(context);
        }

        private static void SeedPackType(IAdaniContext context)
        {
            context.PackTypes.AddOrUpdate(x => x.Id, new Entities.PackType
            {
                Id = 1,
                Name = "Loose",
                IsActive = true,
                SAPCode = "10"
            },
            new Entities.PackType
            {
                Id = 2,
                Name = "Jars",
                IsActive = true,
                SAPCode="40"
            },           
            new Entities.PackType
            {
                Id = 3,
                Name = "Pouches",
                IsActive = true,
                SAPCode="60"
            },
            new Entities.PackType
            {
                Id = 4,
                Name = "Tins",
                IsActive = true,
                SAPCode="30"
            },
            new Entities.PackType
            {
                Id = 5,
                Name = "BIB",
                IsActive = true,
                SAPCode = "20"
            },
            new Entities.PackType
            {
                Id = 6,
                Name = "Bottles",
                IsActive = true,
                SAPCode = "50"
            },
            new Entities.PackType
            {   
                Id = 7,
                Name = "LUPs",
                IsActive = true,
                SAPCode = "70"
            }, new Entities.PackType
            {
                Id = 8,
                Name = "Others",
                IsActive = true,
                SAPCode = "91"
            }, new Entities.PackType
            {
                Id = 9,
                Name = "Box",
                IsActive = true,
                SAPCode = string.Empty
            }, new Entities.PackType
            {
                Id = 10,
                Name = "BAG",
                IsActive = true,
                SAPCode = string.Empty
            }
            //, new Entities.PackType
            //{
            //    Id = 11,
            //    Name = "PET",
            //    IsActive = true,
            //    SAPCode = string.Empty
            //}
            );
        }
    }
}
