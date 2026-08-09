using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FinalPriceSkuOutputDto
    {
        public long PricingId { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public long OilTypeId { get; set; }
        public decimal Price { get; set; }
        public decimal EmployeeSkuPremium { get; set; }
        public long EmployeeSkuPremiumId { get; set; }
        public decimal EmployeeSkuDiscount { get; set; }
        public long EmployeeSkuDiscountId { get; set; }
        public long PlantId { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public long UOMId { get; set; }
        public string UOM { get; set; }
        public long DivisionId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long? OilPackGroupTypeId { get; set; }
    }
}
