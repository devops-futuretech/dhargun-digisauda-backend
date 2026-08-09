using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum SeverityType
    {
       
        [Description("Not able to do anything")] NotAbleToDoAnything = 1,
        [Description("Not able to do a particular transaction")] NotAbleToDoParticularTransaction = 2,
        [Description("Not able to view report")] NotAbleToViewReport = 3
    }
}
