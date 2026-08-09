using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum LiftingFlag
    {
        [Description("Completed")] Completed = 1,
        [Description("Pending")] Pending = 2,
        [Description("Overdue")] Overdue = 3
    }
}
