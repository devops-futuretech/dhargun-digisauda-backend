using System.Data.Entity.Migrations;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO.Enums;
using GMCore.Helper;

namespace Adani.Solution.Data.Seeder
{
    public class RoleHierarchy : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedRoleHierarchy(context);
        }
        private static void SeedRoleHierarchy(IAdaniContext context)
        {
            context.RoleHierarchy.AddOrUpdate(x => x.Id,
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.Admin,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.Admin),    
                    RoleId= (int)DTO.Enums.Role.Admin,
                    HierarchyId = 1,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.BusinessFinanceAdmin,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.BusinessFinanceAdmin),    
                    RoleId= (int)DTO.Enums.Role.BusinessFinanceAdmin,
                    HierarchyId = 2,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.BusinessFinanceManager,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.BusinessFinanceManager),    
                    RoleId= (int)DTO.Enums.Role.BusinessFinanceManager,
                    HierarchyId = 3,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.BusinessFinanceHead,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.BusinessFinanceHead),    
                    RoleId= (int)DTO.Enums.Role.BusinessFinanceHead,
                    HierarchyId = 4,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.Dealer,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.Dealer),    
                    RoleId= (int)DTO.Enums.Role.Dealer,
                    HierarchyId = 5,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.Broker,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.Broker),    
                    RoleId= (int)DTO.Enums.Role.Broker,
                    HierarchyId = 6,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.ZonalTrader,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ZonalTrader),    
                    RoleId= (int)DTO.Enums.Role.ZonalTrader,
                    HierarchyId = 7,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.KAM,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.KAM),    
                    RoleId= (int)DTO.Enums.Role.KAM,
                    HierarchyId = 8,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.ZonalTrader,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ZonalTrader),    
                    RoleId= (int)DTO.Enums.Role.ZonalTrader,
                    HierarchyId = 9,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.HOSalesAdmin,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.HOSalesAdmin),    
                    RoleId= (int)DTO.Enums.Role.HOSalesAdmin,
                    HierarchyId = 10,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.ChiefKAM,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ChiefKAM),    
                    RoleId= (int)DTO.Enums.Role.ChiefKAM,
                    HierarchyId = 11,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.NationalTrader,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.NationalTrader),    
                    RoleId= (int)DTO.Enums.Role.NationalTrader,
                    HierarchyId = 12,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.ASO,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ASO),    
                    RoleId= (int)DTO.Enums.Role.ASO,
                    HierarchyId = 13,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.AreaSalesManager,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.AreaSalesManager),    
                    RoleId= (int)DTO.Enums.Role.AreaSalesManager,
                    HierarchyId = 14,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.RegionalSalesManager,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.RegionalSalesManager),    
                    RoleId= (int)DTO.Enums.Role.RegionalSalesManager,
                    HierarchyId = 15,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.NationalSalesManager,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.NationalSalesManager),    
                    RoleId= (int)DTO.Enums.Role.NationalSalesManager,
                    HierarchyId = 16,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.ITManager,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ITManager),    
                    RoleId= (int)DTO.Enums.Role.ITManager,
                    HierarchyId = 17,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.ITHead,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ITHead),    
                    RoleId= (int)DTO.Enums.Role.ITHead,
                    HierarchyId = 18,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.ShipToParty,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ShipToParty),    
                    RoleId= (int)DTO.Enums.Role.ShipToParty,
                    HierarchyId = 19,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.ABManager,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.ABManager),    
                    RoleId= (int)DTO.Enums.Role.ABManager,
                    HierarchyId = 20,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.SalesExecutive,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.SalesExecutive),    
                    RoleId= (int)DTO.Enums.Role.SalesExecutive,
                    HierarchyId = 21,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.Demonstrator,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.Demonstrator),    
                    RoleId= (int)DTO.Enums.Role.Demonstrator,
                    HierarchyId = 22,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.DemoInCharge,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.DemoInCharge),    
                    RoleId= (int)DTO.Enums.Role.DemoInCharge,
                    HierarchyId = 23,
                    IsActive = true
                },
                new Entities.RoleHierarchy
                {
                    Id = (int)DTO.Enums.Role.SubAdmin,
                    Name = UtilityHelper.GetEnumDescription(DTO.Enums.Role.SubAdmin),    
                    RoleId= (int)DTO.Enums.Role.SubAdmin,
                    HierarchyId = 24,
                    IsActive = true
                }
                );
        }
    }
}
