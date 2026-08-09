using System;
namespace Adani.Solution.DTO
{
    public class ProfitMarginUploadDto :CommonResultDto
    {
        public string StateName { get; set; }
        public string ZoneName { get; set; }
        public string VerticalCode { get; set; }
        //OilWise
        public string OilType { get; set; }
        //BPOrCPWise
        public string OilPackingType { get; set; }
        public string Territory { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public decimal RatePerMt { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        //public string IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
}
