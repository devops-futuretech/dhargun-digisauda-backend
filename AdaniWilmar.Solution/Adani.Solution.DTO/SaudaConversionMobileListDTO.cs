using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConversionMobileListDTO
    {
        public long SkuConversionId { get; set; }
        public string SkuName { get; set; }
        public DateTime ConversionCreatedDate { get; set; }
        public decimal SaudaQuantityInMT { get; set; }
        public string DealerName { get; set; }
        public string BdoName { get; set; }
        public string ZonalHeadName { get; set; }
        public string Remarks { get; set; }
        public decimal SaudaQuantityInSku { get; set; }
        public string PlantOrDepotCode { get; set; }
        public string PlantOrDepotName { get; set; }
        public bool ReprocessStatus { get; set; }
        public bool SaudaConversionUpdateFromSap { get; set; }
        public bool IsSapDataSync { get; set; }
        public long StatusId { get; set; }
        public bool IsReprocessed { get; set; }
    }

    public class SaudaConversionInputDTO : LoginUserIdDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long DealerId { get; set; }
        public int StatusId { get; set; }
    }
}
