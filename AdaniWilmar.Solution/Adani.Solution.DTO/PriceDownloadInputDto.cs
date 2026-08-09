using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class PriceDownloadInputDto
    {
        public long SaudaBookingTypeId { get; set; }
        public long PriceGenerateId { get; set; }
        public long PriceGenerateDetailId { get; set; }
        //public long FinalPriceRecordCount { get; set; }
        public DateTime SearchDate { get; set; }
    }
}
