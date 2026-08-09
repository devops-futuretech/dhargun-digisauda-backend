using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConversionUnitAndDifferenceRateAddDto
    {
        public long FromPackGroupId { get; set; }        
        public long FromSkuId { get; set; }      
        public string FromSkuName { get; set; }      
        public string ToSkuName { get; set; }      
        public decimal FromUnit { get; set; }       
        public DateTime FromDate { get; set; }    
        public DateTime ToDate { get; set; }
        public long LoginUserId { get; set; }
        public List<long> SourceIds { get; set; }
        public List<long> StateIds { get; set; }
        public string StateIdsInString { get; set; }
        public string SourceIdsInString { get; set; }
        public List<SaudaConversionUnitAndDifferenceRateDetailsDto> SaudaConversionUnitAndDifferenceRateDetailsList { get; set; }

        public SaudaConversionUnitAndDifferenceRateAddDto()
        {
            SaudaConversionUnitAndDifferenceRateDetailsList = new List<SaudaConversionUnitAndDifferenceRateDetailsDto>();
        }
    }
}
