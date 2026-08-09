using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class DetentionCost : Auditable
    {
        public long DepotId { get; set; }
        public long DivisionId { get; set; }
        public decimal RatePerMt { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        public virtual Depot Depot { get; set; }
    }
}
