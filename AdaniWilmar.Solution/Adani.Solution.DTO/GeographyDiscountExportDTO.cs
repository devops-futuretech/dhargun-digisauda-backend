using Adani.Solution.DTO.Enums;
using System;
using System.ComponentModel;

namespace Adani.Solution.DTO
{
    public class GeographyDiscountExportDto
    {
        [DisplayName("Discount Id")]
        public long DiscountId { get; set; }
        [DisplayName("Discount")]
        public decimal Discount { get; set; }
        [DisplayName("Valid From")]
        public string ValidFrom { get; set; }
        [DisplayName("Valid To")]
        public string ValidTo { get; set; }
        [DisplayName("Sku Name")]
        public string SkuName { get; set; }
        [DisplayName("Material Code")]
        public string MaterialCode { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [DisplayName("District")]
        public string District { get; set; }
        [DisplayName("State")]
        public string State { get; set; }
        [DisplayName("Zone")]
        public string Zone { get; set; }
        internal int PackTypeId { get; set; }
        [DisplayName("Pack Type")]
        public string PackType
        {
            get
            {
                if (Enum.IsDefined(typeof(BpCpType), PackTypeId))
                    return GetEnumDescription((BpCpType)PackTypeId);
                return "Unknown";
            }
        }
        private string GetEnumDescription(BpCpType value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attr.Length > 0 ? attr[0].Description : value.ToString();
        }

    }
}