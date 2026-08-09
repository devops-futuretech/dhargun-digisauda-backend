using Adani.Solution.Data.DatabaseContext;
using GMCore.Helper;
using System.Data.Entity.Migrations;

namespace Adani.Solution.Data.Seeder
{
    public class Role : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedRole(context);
        }

        private static void SeedRole(IAdaniContext context)
        {
            context.Roles.AddOrUpdate(x => x.Id, new Entities.Role
            {
                Id = (int)DTO.Enums.Role.Admin,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.Admin),
                RoleTypeId = 1,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.BusinessFinanceAdmin,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.BusinessFinanceAdmin),
                RoleTypeId = 2,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.BusinessFinanceManager,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.BusinessFinanceManager),
                RoleTypeId = 3,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.BusinessFinanceHead,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.BusinessFinanceHead),
                RoleTypeId = 4,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.Dealer,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.Dealer),
                RoleTypeId = 5,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.Broker,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.Broker),
                RoleTypeId = 6,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.StateTrader,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.StateTrader),
                RoleTypeId = 7,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.KAM,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.KAM),
                RoleTypeId = 8,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.ZonalTrader,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ZonalTrader),
                RoleTypeId = 9,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.HOSalesAdmin,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.HOSalesAdmin),
                RoleTypeId = 10,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.ChiefKAM,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ChiefKAM),
                RoleTypeId = 11,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.NationalTrader,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.NationalTrader),
                RoleTypeId = 12,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.ASO,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ASO),
                RoleTypeId = 13,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.AreaSalesManager,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.AreaSalesManager),
                RoleTypeId = 14,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.RegionalSalesManager,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.RegionalSalesManager),
                RoleTypeId = 15,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.NationalSalesManager,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.NationalSalesManager),
                RoleTypeId = 16,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.ITManager,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ITManager),
                RoleTypeId = 17,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.ITHead,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ITHead),
                RoleTypeId = 18,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.ShipToParty,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ShipToParty),
                RoleTypeId = 19,
                IsPrime = true
            }
            ,
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.ABManager,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ABManager),
                RoleTypeId = 22,
                IsPrime = true
            }            ,
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.SalesExecutive,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.SalesExecutive),
                RoleTypeId = 21,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.Demonstrator,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.Demonstrator),
                RoleTypeId = 20,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.DemoInCharge,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.DemoInCharge),
                RoleTypeId = 20,
                IsPrime = true
            },
            new Entities.Role
            {
                Id = (int)DTO.Enums.Role.SubAdmin,
                Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.SubAdmin),
                RoleTypeId = 20,
                IsPrime = true
            });
        }
    }
}

