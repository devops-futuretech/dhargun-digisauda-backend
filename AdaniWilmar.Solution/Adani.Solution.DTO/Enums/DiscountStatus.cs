using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum DiscountStatus
    {
        [Description("Approved")] Approved = 1,
        [Description("Pending")] Pending = 2,
        [Description("Canceled")] Canceled = 3,
    }

    public enum RaDiscountType
    {
        [Description("User")] User = 1,
        [Description("Geography")] Geography = 2
    }
}
