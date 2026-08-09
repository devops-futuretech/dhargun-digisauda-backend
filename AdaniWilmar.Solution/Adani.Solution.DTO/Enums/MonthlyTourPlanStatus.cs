using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum MonthlyTourPlanStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("Rejected")]
        Rejected = 3,
        [Description("Drafted")]
        Drafted = 4
    }
}
