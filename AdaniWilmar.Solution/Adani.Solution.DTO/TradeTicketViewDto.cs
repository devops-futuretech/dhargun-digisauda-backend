using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TradeTicketViewDto : EntityDto
    {
        public string ContractType { get; set; }
        public string BookingType { get; set; }
        public string MaterialType { get; set; }
        public string UnitOfMeasure { get; set; }
        public string PlantOrVendor { get; set; }


        public string TradeTicketNumber { get; set; }
        public long TradeTicketId { get; set; }
        public long UomId { get; set; }
        public long VerticalId { get; set; }
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
        public DateTime ContractDate { get; set; }

        public long DepotId { get; set; }
        public int ContractTypeId { get; set; }
        public int MaterialTypeId { get; set; }
        public int BookingTypeId { get; set; }
        public decimal ContractQuantity { get; set; }
        public decimal OtherElement { get; set; }
        public decimal SaudaBookedQuantity { get; set; }
        public decimal TotalOilCost { get; set; }
        public decimal TotalProcessCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalOtherElement { get; set; }

        public decimal OpenQty { get; set; }
        public string PlantName { get; set; }
        public string TradeTicketOilTypes { get; set; }
        public DateTime SAPCreationDate { get; set; }
        public decimal RatePerMT { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValidFrom { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValidTo { get; set; }

        public bool IsSAPDataSync { get; set; }
        public bool IsModified { get; set; }
        public List<TradeTicketDetailsDto> TradeTicketDetail { get; set; }

        public TradeTicketViewDto()
        {
            TradeTicketDetail = new List<TradeTicketDetailsDto>();
        }
    }

    public class TradeTicketSaudaMapping
    {
        public long TradeTicketId { get; set; }
    }

    public class TradeTicketParamDto : LoginUserIdDto
    {
        public DateTime SearchDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Vertical { get; set; }
    }
    public class TradeTicketDeleteDto : IAPIInputDTO
    {
        public int TradeTicketId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class TradeTicketSaudaUnMappingDto : LoginUserIdDto, IAPIInputDTO
    {
        public long SaudaId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
