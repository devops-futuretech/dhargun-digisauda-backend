using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FinalPricePublishDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long LoginUserId { get; set; }
        public long BiddingWindowId { get; set; }
        public DateTime BiddingDate { get; set; }
        public long PublishId { get; set; }
        public long BookingTypeId { get; set; }
        public long FinalPriceRecordCount { get; set; }
        public int SkipCount { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public DateTime SearchDate { get; set; }

        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
}
