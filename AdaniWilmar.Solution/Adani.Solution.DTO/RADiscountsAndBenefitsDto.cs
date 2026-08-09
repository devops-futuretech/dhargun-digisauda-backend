using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class RADiscountsAndBenefitsDto 
    {
        public string DealerName { get; set; }
        public List<SkuDiscountDto> SkuDiscount { get; set; }
        public List<SkuDiscountDto> SchemeDiscount { get; set; }
        public List<RAVolumeDiscountDto> VolumeDiscount { get; set; }
        public RADiscountsAndBenefitsDto()
        {
            SkuDiscount = new List<SkuDiscountDto>();
            SchemeDiscount = new List<SkuDiscountDto>();
            VolumeDiscount = new List<RAVolumeDiscountDto>();
        }
    }
    public class SkuDiscountDto
    {
        public decimal Discount { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }

    public class RAVolumeDiscountDto
    {
        public decimal Discount { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal StartVolumeSlabInMT { get; set; }
        public decimal EndVolumeSlabInMT { get; set; }
    }
}
