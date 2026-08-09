using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum Role
    {
        [Description("Admin")] Admin = 1,
        [Description("Business Finance Admin")] BusinessFinanceAdmin = 2,
        [Description("Business Finance Manager")] BusinessFinanceManager = 3,
        [Description("Business Finance Head")] BusinessFinanceHead = 4,
        [Description("Distributor")] Dealer = 5,
        [Description("Broker")] Broker = 6,
        [Description("State Trader")] StateTrader = 7,
        [Description("KAM")] KAM = 8,
        [Description("Zonal Trader")] ZonalTrader = 9,
        [Description("HO Sales Admin")] HOSalesAdmin = 10,
        [Description("Chief KAM")] ChiefKAM = 11,
        [Description("National Trader")] NationalTrader = 12,
        [Description("ASO")] ASO = 13,
        [Description("Area Sales Manager")] AreaSalesManager = 14,
        [Description("Regional Sales Manager")] RegionalSalesManager = 15,
        [Description("National Sales Manager")] NationalSalesManager = 16,
        [Description("IT Manager")] ITManager = 17,
        [Description("IT Head")] ITHead = 18,
        [Description("ShipToParty")] ShipToParty = 19,
        [Description("Associate Branch Manager")] ABManager = 20,
        [Description("Sales Executive")] SalesExecutive = 21,
        [Description("Demonstrator")] Demonstrator = 22,
        [Description("Demo In-Charge")] DemoInCharge = 23,
        //production id
        [Description("Sub Admin")] SubAdmin = 24
    }
}
