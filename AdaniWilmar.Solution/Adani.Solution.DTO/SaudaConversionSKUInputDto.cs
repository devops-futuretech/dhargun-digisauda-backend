using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConversionSKUInputDto :LoginUserIdDto
    {
        public long SkuId { get; set; }
        public long SaudaConversionId { get; set; }        
        public decimal QuantityInSku { get; set; }
        public decimal QuantityInMt { get; set; }
        public long OilTypeId { get; set; }
        public long DealerId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public string Remarks { get; set; }
        public long PlantOrDepotId { get; set; }
        public List<SaudaConvertedToSkuDto> SaudaConvertedToSkuList { get; set; }

        public SaudaConversionSKUInputDto()
        {
            SaudaConvertedToSkuList = new List<SaudaConvertedToSkuDto>();
        }
    }

    public class SaudaConvertedToSkuDto
    {
        public long SkuId { get; set; }
        public decimal QuantityInSku { get; set; }
        public decimal QuantityInMt { get; set; }
        public long SaudaConversionUnitAndDifferenceRateDetailsId { get; set; }
    }
}
