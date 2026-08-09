
using System.ComponentModel;


namespace Adani.Solution.DTO.Enums
{
    public enum DSRReportType
    {
        [Description("DealerVisit")] DealerVisit = 1,
        [Description("Wholesaler")] Wholesaler = 2,
        [Description("ProspectiveDealer")] ProspectiveDealer = 3,
    }
}