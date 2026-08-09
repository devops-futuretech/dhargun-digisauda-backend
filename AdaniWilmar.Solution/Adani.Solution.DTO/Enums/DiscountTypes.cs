using System.ComponentModel;

namespace Adani.Solution.DTO.Enums
{
    public enum DiscountTypes
    {
        [Description("Customer")] Customer = 1,
        [Description("Product")] Product = 2,
        [Description("Geography ")] Geography = 3,
    }

    public enum RADiscountTypes
    {
        [Description("VolumeDiscountUser")] VolumeDiscountUser = 1,
        [Description("VolumeDiscountGeography")] VolumeDiscountGeography = 2,
        [Description("SchemeDiscountUser")] SchemeDiscountUser = 3,
        [Description("SchemeDiscountGeography")] SchemeDiscountGeography = 4,
        [Description("SkuDiscountUser")] SkuDiscountUser = 5,
        [Description("SkuDiscountGeography")] SkuDiscountGeography = 6,
        [Description("GPBenefitUser")] GPBenefitUser = 7,
        [Description("GPBenefitGeogfraphy")] GPBenefitGeogfraphy = 8,
        [Description("GST")] GST = 9,
    }

    public enum DiscountType
    {
        //[Description("User")] User = 1,
        [Description("Geography ")] Geography = 2,
    }
}
