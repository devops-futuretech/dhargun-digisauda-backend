using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TradeTicketInputDto :IAPIInputDTO
    {
        public long TradeTicketId { get; set; }
        public long UomId { get; set; }
        public long VerticalId { get; set; }
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
        public DateTime ContractDate { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public long DepotId { get; set; }
        public int ContractTypeId { get; set; }
        public int MaterialTypeId { get; set; }
        public int BookingTypeId { get; set; }
        public decimal ContractQuantity { get; set; }
        public decimal OtherElement { get; set; }
        public string PlantOrVendor { get; set; }
        public decimal TotalOilCost { get; set; }
        public decimal TotalProcessCost { get; set; }
        public decimal TotalCost { get; set; }
        public List<TradeTicketDetailsDto> TradeTicketDetails { get; set; }

        public TradeTicketInputDto()
        {
            TradeTicketDetails = new List<TradeTicketDetailsDto>();
        }
               
        public decimal Proporion { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
