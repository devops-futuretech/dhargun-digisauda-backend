using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class CustomerShipToPartyMapping : Auditable
    {
        [Required]
        public long CustomerId { get; set; }
        public long ShipToPartyId { get; set; }

        public virtual User Customer { get; set; }
        public virtual User ShipToParty { get; set; }
    }
}
