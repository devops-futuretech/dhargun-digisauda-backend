using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum PackGroupType
    {
        [Description("Premium")] Premium = 1,
        [Description("Bakery")] Bakery = 2,
        [Description("Popular")] Popular = 3,
        [Description("Lauric")] Lauric = 4
    }
}
