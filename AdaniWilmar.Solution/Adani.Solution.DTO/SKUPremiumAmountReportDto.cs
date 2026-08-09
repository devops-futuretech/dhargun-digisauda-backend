using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SKUPremiumAmountReportDto
    {
        public string Divisions { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public decimal PremiumAmount { get; set; }
        
    }
}
