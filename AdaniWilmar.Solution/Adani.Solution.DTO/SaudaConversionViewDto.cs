using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SaudaConversionViewDto
    {
        public long SaudaConversionId { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public long PlantId { get; set; }
        public string Plant { get; set; }
        public long OldSkuId { get; set; }
        public string OldMaterialNumber { get; set; }
        public decimal OldQuantityInCase { get; set; }
        public decimal OldQuantityInMT { get; set; }
        public long NewSkuId { get; set; }
        public string NewMaterialNumber { get; set; }
        public decimal NewQuantityInCase { get; set; }
        public decimal NewQuantityInMT { get; set; }
        public decimal PROO { get; set; }
        public decimal FRC1 { get; set; }
        public decimal PrimaryFright { get; set; }
        public decimal ToUnit { get; set; }
        public string PackGroup { get; set; }
    }

    public class HANASaudaConversion
    {
        public List<HANASaudaConversionViewDto>  Header { get; set; }
        public  HANASaudaConversion()
        {
            Header = new List<HANASaudaConversionViewDto>();
        }
    }
        public class HANASaudaConversionViewDto
    {
        public long SaudaConversionId { get; set; }
        public string Dealer { get; set; }
        public string Plant { get; set; }
        public string OldMaterialNumber { get; set; }
        public decimal OldQuantityInCase { get; set; }
        public string NewMaterialNumber { get; set; }
        public decimal NewQuantityInCase { get; set; }
        public decimal PROO { get; set; }
        public decimal FRC1 { get; set; }
        public decimal PrimaryFright { get; set; }
        public decimal ToUnit { get; set; }
        public string PackGroup { get; set; }
    }
}
