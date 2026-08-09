using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SurpriseBenefitInputDto
    {
        public string SkuIdString { get; set; }
        public List<long> SkuIds { get; set; }

        public long BDOId { get; set; }

        public long CustomerGroupId { get; set; }
        public string CustomerIdString { get; set; }
        public List<long> CustomerIds { get; set; }

        public decimal PercentileNumber { get; set; }
        public decimal SurpriseDiscount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public long BenefitTypeId { get; set; }
        public long BenefitOrCategoryId { get; set; }
        public string BenefitType { get; set; }
        public string BenefitOrCategory { get; set; }

        public long SapDays { get; set; }
        public decimal NonSapDiscount { get; set; }
    }
}
