using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SurpriseBenefitGeographyExportDto
    {
        public string CustomerGroup { get; set; }
        public string OilTypeName { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string PackGroup { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string CityName { get; set; }
        public string BenefitType { get; set; }
        public long SapBenefitDays { get; set; }
        public decimal NonSapDiscountPerCase { get; set; }
        public bool IsActive { get; set; }
    }
}
