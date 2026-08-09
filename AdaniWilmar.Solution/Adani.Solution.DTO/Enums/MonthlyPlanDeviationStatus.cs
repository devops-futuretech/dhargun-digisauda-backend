using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum MonthlyPlanDeviationStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("Rejected")]
        Rejected = 3
    }
}
