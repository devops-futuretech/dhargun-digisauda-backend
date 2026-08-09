using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum SaudaConversionType
    {
        [Description("CPToCP")] CPToCP = 1,
        [Description("BPToBP")] BPToBP = 2,
        [Description("BPToCP")] BPToCP = 3,
        [Description("CPToBP")] CPToBP = 4,
    }
}