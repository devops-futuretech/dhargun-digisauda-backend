using System;
using System.Collections.Generic;
using System.Linq;
using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class ContractType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            SeedContractType(context);
        }

        private static void SeedContractType(IAdaniContext context)
        {
            context.ContractTypes.AddOrUpdate(x => x.Id, new Entities.ContractType
            {
                Id = 1,
                Name = "Own",
                Code = "ZHQ",
                IsActive = true,
            },
            new Entities.ContractType
            {
                Id = 2,
                Name = "Third Party",
                Code = "ZSFQ",
                IsActive = true,
            }
            //,
            //new Entities.ContractType
            //{
            //    Id = 3,
            //    Name = "Rasoi",
            //    Code = "ZRQ",
            //    IsActive = true,
            //}
            );
        }
    }
}
