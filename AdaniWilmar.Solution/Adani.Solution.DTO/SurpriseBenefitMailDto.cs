using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SurpriseBenefitMailDto
    {
        public List<long?> BDOIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> SkuIds { get; set; }
        public long CustomerGroupId { get; set; }
        public long BenefitTypeId { get; set; }
        public string BenefitType { get; set; }
        public long BenefitOrCategoryId { get; set; }
        public string BenefitOrCategory { get; set; }
        public decimal BenefitDiscountOrDays { get; set; }
        public string DiscountOrDays { get; set; }
    }
}
