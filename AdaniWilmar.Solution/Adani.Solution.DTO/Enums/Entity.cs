using System.ComponentModel;


namespace Adani.Solution.DTO.Enums
{
    public enum Entity
    {
        [Description("OilType")] OilType = 1,
        [Description("Material")] Sku = 2,
        [Description("User")] User = 3,
        [Description("Broker")] Broker = 4,
        [Description("ShipToParty")] ShipToParty = 5,
        //[Description("Material Type")] MaterialType = 6,
        //[Description("Volume Loadability")] VolumeLoadability = 7,
        [Description("User Customer Sales Target")] UserCustomerSalesTarget = 8,
        [Description("User Customer Sauda Target")] UserCustomerSaudaTarget = 9,
        [Description("Plant")] Plant = 10,
        //[Description("Depot")] Depot = 11,
        //[Description("Plant Depot Mapping")] PlantDepotMapping = 12,
        //[Description("User Depot Mapping")] UserDepotMapping = 13,
        [Description("User Customer Mapping")] UserCustomerMapping = 14,
        [Description("Pending Sauda")] PendingSauda = 15,
        [Description("Geography")] Geography = 16,
        [Description("Customer Master")] CustomerMaster = 17,
        [Description("User Customer Target")] UserCustomerTarget = 18,
        //[Description("TradeTicket")] TradeTicket = 19,
        //[Description("Vehicle Loadability")] VehicleLoadability = 20,
        //[Description("Sauda Conversion Unit And Base Rate Difference")] SaudaConversionUnitAndBaseRateDifference = 21,
        //[Description("CustomerGroup")] CustomerGroup = 22,
        //[Description("PercentileNumbers")] PercentileNumbers = 23,
        //[Description("GST")] GST = 24,
        [Description("CustomerGroup Five")] CustomerGroupFive = 25,
        [Description("Sales Organization")] SalesOrganization = 30,
        [Description("Distribution Channel")] DistributionChannel = 31,
        [Description("Division")] Division = 32,
        [Description("User Division Mapping")] UserDivisionMapping = 33,
        [Description("DealerSaudaValidity")] DealerSaudaValidity = 26,
        [Description("DealerSaudaLimit")] DealerSaudaLimit = 27,
        [Description("Dealer Call Recording Details")] DealerCallRecordingDetails = 28,
        [Description("Broker Call Recording Details")] BrokerCallRecordingDetails = 29,
        [Description("User Discount")] UserDiscount = 34,
        [Description("Geography Discount")] GeographyDiscount = 35,
        [Description("Line")] Line = 36,
        [Description("QPS Discount")] QPSDiscount = 37,
        [Description("SKU BPCP Mapping")] SKUBPCPMapping = 38,
        [Description("Gamification Dashboard")] GamificationDashboard = 39,
        [Description("Quantity Limit")] QuantityLimit = 40,
        [Description("Sauda Conditional Booking")] SaudaConditionalBookingConfiguration = 41
    }
}
