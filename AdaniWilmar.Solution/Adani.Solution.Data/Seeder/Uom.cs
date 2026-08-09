using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class Uom : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedUom(context);
        }

        private static void SeedUom(IAdaniContext context)
        {
            context.Uom.AddOrUpdate(x => x.Id, new Entities.Uom
            {
                Id = 1,
                Name = "Case",
                IsQuantityType = false,
                IsActive = true,
                SAPName= "CAR"
            },
            new Entities.Uom
            {
                Id = 2,
                Name = "Ltr",
                IsQuantityType = true,
                IsActive = true,
                SAPName="L"
            },
            new Entities.Uom
            {
                Id = 3,
                Name = "MT",
                IsQuantityType = false,
                IsActive = true,
                SAPName= "MT"
            },
            new Entities.Uom
            {
                Id = 4,
                Name = "Kg",
                IsQuantityType = true,
                IsActive = true,
                SAPName= "KG"
            },
            new Entities.Uom
            {
                Id = 5,
                Name = "NOS",
                IsQuantityType = false,
                IsActive = true,
                SAPName= "NOS"
            },
            new Entities.Uom
            {
                Id = 6,
                Name = "Each",
                IsQuantityType = false,
                IsActive = true,
                SAPName="EA"
            },            
            new Entities.Uom
            {
                Id = 7,
                Name = "BAG",
                IsQuantityType = false,
                IsActive = true,
                SAPName = "BAG"
            }
            );
        }
    }
}
