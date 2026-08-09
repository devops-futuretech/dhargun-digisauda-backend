using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class SaudaBiddingCartHeader : Auditable
    {
        public long BiddingWindowId { get; set; }
        public long DealerId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime BiddingDateAndTime { get; set; }
        public virtual User Dealer { get; set; }
        public virtual BiddingWindow BiddingWindow { get; set; }
    }
}
