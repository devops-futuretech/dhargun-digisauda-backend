using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum MarginReport
    {
        [Description("Plant wise Sauda")] PlantwiseSauda = 1,
        [Description("State & Oil Margin")] StateOilMargin = 2,
        [Description("Business Margin")] BusinessMargin = 3,
        [Description("Sauda Report")] SaudaReport = 4
    }
}
