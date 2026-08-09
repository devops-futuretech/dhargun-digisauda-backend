using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum SupportStatus
    {
        [Description("Open")] Open = 1,
        [Description("InProgress")] InProgress = 2,
        [Description("Resolved")] Resolved = 3,
        [Description("Closed")] Closed = 4,
        [Description("Reopen")] Reopen = 5
    }
}
