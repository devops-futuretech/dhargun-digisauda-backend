using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum IssueRaisedUser
    {
        [Description("EAL Employee")] EALEmployee = 1,
        [Description("Distributors")] Distributors = 2,
        [Description("All")] All = 3
    }
}
