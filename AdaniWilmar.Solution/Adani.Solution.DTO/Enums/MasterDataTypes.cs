using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum MasterDataTypes
    {
        [Description("Sauda Status")] SaudaStatus = 1,
        [Description("Sauda Booking Type")] SaudaBookingType = 2,
        [Description("Oil Packing Type")] OilPackingType = 3,
        [Description("Pack Type")] PackType = 4,
        [Description("Transaport Mode")] TransaportMode = 5       
    }
}
