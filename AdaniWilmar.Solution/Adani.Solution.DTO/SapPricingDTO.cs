using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SapPricingDTO
    {
        public string Condition_RecordNo { get; set; }
        public string Material { get; set; }
        public string FromPlant { get; set; }
        public string DepotCode { get; set; }
        public decimal Amount { get; set; }       
        public string SalesOrg { get; set; }
        public string Division { get; set; }
        public string Distributor_Channel { get; set; }
        public string Valid_From { get; set; }
        public string Valid_To { get; set; }
        public decimal PricingUnit { get; set; }
        public string Unit_Of_Measure { get; set; }
        public string Condition_Currency { get; set; }
    }

    public class HANAPricing
    {
        public List<SapPricingDTO> PriceControl_Details { get; set; }

        public HANAPricing()
        {
            PriceControl_Details = new List<SapPricingDTO>();
        }
    }
}
