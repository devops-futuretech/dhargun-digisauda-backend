using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class LiftingRequestDetails : Auditable
    {
        [Required]
        public long LiftingRequestId { get; set; }

        public long SkuId { get; set; }
        [Required]
        public long OilTypeId { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal LiftingQuantity { get; set; }
        public decimal LiftingQuantityCase { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string ItemNo { get; set; }
        public int StatusId { get; set; }
        public long UomId { get; set; }
        public int DOStatusId { get; set; }

        public string Remarks { get; set; }

        public string EnquiryNumber { get; set; }
        public string EnquiryRemarks { get; set; }
        public bool ReprocessStatusId { get; set; }
        public bool EnquiryNumberSyncFromSap { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionhannelId { get; set; }
        public long DivisionId { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual LiftingRequest LiftingRequest { get; set; }
        public virtual Sku Sku { get; set; }
    }
}
