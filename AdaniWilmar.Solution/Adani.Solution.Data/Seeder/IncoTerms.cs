using System;
using System.Collections.Generic;
using System.Linq;
using Adani.Solution.Data.DatabaseContext;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class IncoTerms : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedIncoTerms(context);
        }

        private static void SeedIncoTerms(IAdaniContext context)
        {
            context.IncoTerms.AddOrUpdate(x => x.Id, new Entities.IncoTerms
            {
                Id = 1,
                Name = "For Plant",
                Code = "For Plant",
                IsActive = true,
                Type = 1,
                SAPName = "FOR"
            }
            //},
            //new Entities.IncoTerms
            //{
            //    Id = 2,
            //    Name = "Ex Plant",
            //    Code = "Ex Plant",
            //    IsActive = true,
            //    Type = 1,
            //    SAPName="EXW"
            //}//,
            //new Entities.IncoTerms
            //{
            //    Id = 3,
            //    Name = "For Depot",
            //    Code = "For Depot",
            //    IsActive = true,
            //    Type = 2,
            //    SAPName="FOR"
            //},
            //new Entities.IncoTerms
            //{
            //    Id = 4,
            //    Name = "Ex Depot",
            //    Code = "Ex Depot",
            //    IsActive = true,
            //    Type = 2,
            //    SAPName="EXW"
            //},
            //new Entities.IncoTerms
            //{
            //    Id = 5,
            //    Name = "For Rake",
            //    Code = "For Rake",
            //    IsActive = true,
            //    Type = 3,
            //    SAPName= "FOK"
            //},
            //new Entities.IncoTerms
            //{
            //    Id = 6,
            //    Name = "Ex Rake",
            //    Code = "Ex Rake",
            //    IsActive = true,
            //    Type = 3,
            //    SAPName="EXR"
            //}
            );
        }
    }
}
