using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaInputDto
    {
        public int SaudaType { get; set; }
        public long LoginUserId { get; set; }
        public long BDOId { get; set; }
        public long DealerId { get; set; }
        public DateTime BiddingDate { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long DealerTypeId { get; set; }
        public long BrokerId { get; set; }
        public bool IsDiscountAllocationOver { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public bool IsCrossAndUpsellContract { get; set; }


        public List<SaudaOrderInputDto> SaudaOrders { get; set; }

        public  SaudaInputDto()
        {
            SaudaOrders = new List<SaudaOrderInputDto>();
        }
    }

    public class SaveDealerDetails
    {
        public long DealerId { get; set; }
        public string DealerMobileNumber { get; set; }
        public long BDOId { get; set; }
        public string BDOMobileNumber { get; set; }
    }
}
