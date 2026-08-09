using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TradeTicketUploadDto : CommonResultDto
    {
        public string ContractType { get; set; }
        public string BookingType { get; set; }
        public string MaterialType { get; set; }
        public decimal ContractQuantityInMT { get; set; }
        public string UnitOfMeasurement { get; set; }
        public string PlantCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ContractDate { get; set; }
        public decimal OtherElementsInRsPerMT { get; set; }
        public string TradeDetails_OT_OilCost_Proportion { get; set; }
        public decimal ProcessCost { get; set; }
        public long CreatedBy { get; set; }
    }
}
