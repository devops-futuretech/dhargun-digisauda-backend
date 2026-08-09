using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConversionDetailsBySkuId
    {        
        public long SkuConversionId { get; set; }
        public string BDOName { get; set; }
        public string DealerName { get; set; }        
        public DateTime ConversionCreatedDate { get; set; }
        public DateTime ConversionModifiedDate { get; set; }
        public string SaudaConversionStatus { get; set; }
        public long SaudaConversionStatusId { get; set; }
        public string ZonalHeadName { get; set; }
        public string Remarks { get; set; }
        public string SkuName { get; set; }
        public decimal SaudaQuantityInSku { get; set; }
        public decimal SaudaQuantityInMT { get; set; }
        public string PlantOrDepotCode { get; set; }
        public string PlantOrDepotName { get; set; }
        public List<SaudaConversionSkuDetailOutput> FromSkus { get; set; }
        public List<SaudaConversionSkuDetailOutput> ToSkus { get; set; }
        public SaudaConversionDetailsBySkuId()
        {
            FromSkus = new List<SaudaConversionSkuDetailOutput>();
            ToSkus = new List<SaudaConversionSkuDetailOutput>();
        }

    }
    public class SaudaConversionSkuDetailOutput
    {
        public long SaudaConversionId { get; set; }
        public long SaudaConversionDetailId { get; set; }
        public string SkuName { get; set; }
        public string SaudaNumber { get; set; }
        public decimal SaudaQuantityInMT { get; set; }
        public decimal? BaseRate { get; set; }
        public string Remarks { get; set; }
        public decimal SaudaQuantityInSku { get; set; }
    }
}
