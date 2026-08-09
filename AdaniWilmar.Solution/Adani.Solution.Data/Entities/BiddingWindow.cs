using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class BiddingWindow : Auditable
    {
        public BiddingWindow()
        {
            this.BiddingWindowVolumeCapacity = new HashSet<BiddingWindowVolumeCapacity>();
        }

        public string Name { get; set; }

        public long BiddingWindowCustomerGroupId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BiddingDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndTime { get; set; }

        public int NoOfAttemptsForBidding { get; set; }

        public bool IsActive { get; set; }

        public int StatusId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime SkuAllocationTimeLimit { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime SaudaAllocationStartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime SaudaAllocationEndTime { get; set; }

        public int SaudaAllocationStatusId { get; set; }

        public virtual ICollection<BiddingWindowCustomerGroups> BiddingWindowCustomerGroups { get; set; }

        public virtual ICollection<BiddingWindowVolumeCapacity> BiddingWindowVolumeCapacity { get; set; }
    }
}
