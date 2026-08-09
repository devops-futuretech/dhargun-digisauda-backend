using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class TradeTicketDetails: Auditable
    {
        [Required]
        public long TradeTicketId { get; set; }
        [Required]
        public long TradeTicketOilTypeId { get; set; }
        [Required]
        public decimal ProcessCost { get; set; }
        [Required]
        public decimal Proportion { get; set; }
        [Required]
        public decimal OilCost { get; set; }

        public virtual TradeTicket TradeTicket { get; set; }
        public virtual TradeTicketOilType TradeTicketOilType { get; set; }
    }
}
