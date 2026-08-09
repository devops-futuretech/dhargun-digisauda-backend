using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum IssueType
    {
        [Description("Knowledge")] Knowledge = 1,
        [Description("Application")] Application = 2,
        [Description("Interface")] Interface = 3,
        [Description("Enhancement")] Enhancement = 4
    }
}
