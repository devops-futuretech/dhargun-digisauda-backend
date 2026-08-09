using System.Data.Entity.Migrations;
using Adani.Solution.Data.DatabaseContext;

namespace Adani.Solution.Data.Seeder
{
    public class RoleType : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedRoleType(context);
        }

        private static void SeedRoleType(IAdaniContext context)
        {
            context.RoleTypes.AddOrUpdate(x => x.Id, new Entities.RoleType
            {
                Id = 1,
                Name = "Admin",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 2,
                Name = "Business Finance Admin",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 3,
                Name = "Business Finance Manager",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 4,
                Name = "Business Finance Head",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 5,
                Name = "Distributor",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 6,
                Name = "Broker",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 7,
                Name = "State Trader",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 8,
                Name = "KAM",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 9,
                Name = "Zonal Trader",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 10,
                Name = "HO Sales Admin",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 11,
                Name = "Chief KAM",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 12,
                Name = "National Trader",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 13,
                Name = "ASO",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 14,
                Name = "Area Sales Manager",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 15,
                Name = "Regional Sales Manager",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 16,
                Name = "National Sales Manager",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 17,
                Name = "IT Manager",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 18,
                Name = "IT Head",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 19,
                Name = "ShipToParty",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 20,
                Name = "Demonstrator",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 21,
                Name = "Sales Executive",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 22,
                Name = "Associate Branch Manager",
                IsPrime = true
            },
            new Entities.RoleType
            {
                Id = 23,
                Name = "Sub Admin",
                IsPrime = true
            });
        }
    }
}
