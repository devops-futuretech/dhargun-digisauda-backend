using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaModificationStatusListDto
    {
        public List<DealerGroupedSaudaModificationDto> PendingList { get; set; }
        public List<DealerGroupedSaudaModificationDto> ApprovedList { get; set; }

        public SaudaModificationStatusListDto()
        {
            PendingList = new List<DealerGroupedSaudaModificationDto>();
            ApprovedList = new List<DealerGroupedSaudaModificationDto>();
        }
    }

    public class DealerGroupedSaudaModificationDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public List<SaudaModificationListItemDto> Items { get; set; }
    }

    public class SaudaModificationListItemDto
    {
        public long Id { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public string CreatedByName { get; set; }
        public DateTime ModificationDate { get; set; }
        public string SaudaNumber { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime BiddingDate { get; set; }
        public string ApprovalRejectedByName { get; set; }

    }
}


