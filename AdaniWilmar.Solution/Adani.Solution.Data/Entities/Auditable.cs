using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public abstract class Auditable
    {
        public long Id { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedDate { get; set; }
    }
}
