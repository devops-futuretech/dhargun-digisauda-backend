using Adani.Solution.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class SaudaLimit : Auditable
    {
        [Required]
        public long UserId { get; set; }
        public string UserCode { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal ActualLimit { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal RequestedLimit { get; set; }

        public long StatusId { get; set; }
        public string Remarks { get; set; }
        public virtual User User { get; set; }
        public bool IsSAPData { get; set; }
        public bool IsSAPDataSyncOrNot { get; set; }
        public decimal PendingContract { get; set; }
        public decimal PendingDO { get; set; }
        public decimal PendingOBD { get; set; }   
        public string Division { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal LimitQty { get; set; }
        public string UOM { get; set; }
        public decimal TargetValue { get; set; }
        public string Currency { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndDate { get; set; }
        public decimal OldQty { get; set; }
        public decimal OldValue { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
    }
}
