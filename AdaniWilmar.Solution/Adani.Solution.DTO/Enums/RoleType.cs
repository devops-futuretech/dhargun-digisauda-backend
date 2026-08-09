using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum RoleType
    {
        [Description("Admin")] Admin = 1,
        [Description("Business Finance Admin")] BusinessFinanceAdmin = 2,
        [Description("Business Finance Manager")] BusinessFinanceManager = 3,
        [Description("Business Finance Head")] BusinessFinanceHead = 4,
        [Description("Dealer")] Dealer = 5,
        [Description("Broker")] Broker = 6,
        [Description("StateTrader")] StateTrader = 7,
        [Description("KAM")] KAM = 8,
        [Description("Zonal Head")] ZonalTrader = 9,
        [Description("HO Sales Admin")] HOSalesAdmin = 10,
        [Description("Chief KAM")] ChiefKAM = 11,
        [Description("National Head")] NationalTrader = 12,
        [Description("ASO")] ASO = 13,
        [Description("Area Sales Manager")] AreaSalesManager = 14,
        [Description("Regional Sales Manager")] RegionalSalesManager = 15,
        [Description("National Sales Manager")] NationalSalesManager = 16,
        [Description("IT Manager")] ITManager = 17,
        [Description("IT Head")] ITHead = 18,
    }
}
