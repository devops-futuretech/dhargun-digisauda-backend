using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Sauda : Auditable
    {
        public long UserId { get; set; }           
        [Column(TypeName = "datetime2")]
        public DateTime BiddingDate { get; set; }
        public bool IsSAPDataSync { get; set; }
        public bool IsSAPDataSyncApproval { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public long BdoId { get; set; }
        public string SalesDocumentType { get; set; }

        [Required]
        public long SaudaBookingTypeId { get; set; }
        public string SaudaNumber { get; set; }
        public int SaudaType { get; set; }
        public bool IsSapSauda { get; set; }
        public int StatusId { get; set; }
        public long SpecialRateRequestIdInParentTable { get; set; }
        public bool IsCrossAndUpsellContract { get; set; }
        public virtual Division Division { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual SaudaBookingType SaudaBookingType { get; set; }
    }
}
