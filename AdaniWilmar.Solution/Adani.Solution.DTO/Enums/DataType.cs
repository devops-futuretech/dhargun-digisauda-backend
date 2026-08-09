using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum DataType
    {
        [Description("String")] String = 1,
        [Description("Int")] Int = 2,
        [Description("Boolean")] Boolean = 3,
        [Description("Decimal")] Decimal = 4
    }
}
