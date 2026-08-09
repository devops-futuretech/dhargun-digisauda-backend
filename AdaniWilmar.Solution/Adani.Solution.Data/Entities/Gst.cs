using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class Gst : Auditable
    {
        public long DepotId { get; set; }

        public long OilTypeId { get; set; }

        public int SourceStateId { get; set; }

        public int DestinationStateId { get; set; }

        public decimal CGST { get; set; }

        public decimal SGST { get; set; }

        public decimal IGST { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        public virtual State SourceState { get; set; }

        public virtual State DestinationState { get; set; }

        public long ParentId { get; set; }
    }
}
