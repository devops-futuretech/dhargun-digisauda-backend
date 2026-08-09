using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SaudaConversionUnitAndDifferenceRateDetailsDto
    {
        public long SaudaConversionUnitAndDifferenceRatesId { get; set; }       
        public long ToPackGroupId { get; set; }      
        public string ToPackGroupName { get; set; }      
        public long ToSkuId { get; set; }       
        public string ToSkuName { get; set; }       
        public decimal ToUnit { get; set; }        
        public decimal BasicRate { get; set; }
    }
}
