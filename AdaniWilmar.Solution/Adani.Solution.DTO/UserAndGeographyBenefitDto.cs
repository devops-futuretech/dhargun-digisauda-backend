using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserAndGeographyBenefitDto
    {
        public long SaudaOrderId { get; set; }
        public long BenefitTypeId { get; set; }
        public string BenefitType { get; set; }
        public string Benefit { get; set; }
        public long BenefitOrCategoryId { get; set; }
        public long BenefitUserId { get; set; }
        public decimal BenefitDiscountOrDays { get; set; }
        public bool IsGPBenefit { get; set; }
        public bool IsSurpriseBenefit { get; set; }
    }
}
