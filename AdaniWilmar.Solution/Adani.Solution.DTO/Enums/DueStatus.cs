using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum DueStatus
    {
        [Description("Pending Due")] PendingDue = 1,
        [Description("Over Due")] OverDue = 2,
    }
}
