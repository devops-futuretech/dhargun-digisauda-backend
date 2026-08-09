using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class DistributorStockEntry : Auditable
    {
        public long UserId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ReportedDate { get; set; }
        public virtual User User { get; set; }
    }
}
