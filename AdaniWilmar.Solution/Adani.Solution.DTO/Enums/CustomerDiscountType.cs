using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum CustomerDiscountType
    {
        [Description("Customer")] Customer = 1,
        [Description("Product")] Product = 2,
    }
}
