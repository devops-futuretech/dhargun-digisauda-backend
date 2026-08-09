using System;
using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class BenefitTypes : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedBenefitTypes(context);
        }

        private static void SeedBenefitTypes(IAdaniContext context)
        {
            context.BenefitTypes.AddOrUpdate(x => x.Id, new Entities.BenefitTypes
            {
                Id = 1,
                Name = "SAP",
                IsActive = true,
            },
            new Entities.BenefitTypes
            {
                Id = 2,
                Name = "NON-SAP",
                IsActive = true,
            });
        }
    }
}
