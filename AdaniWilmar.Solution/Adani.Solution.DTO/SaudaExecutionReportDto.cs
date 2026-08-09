using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaExecutionReportDto
    {
        public string AppBookingId { get; set; }
        public string SkuCode { get; set; }
        public string Plant { get; set; }
        public string Division { get; set; }
        public string SaudaNumber { get; set; }
        public string SaudaBookedBy { get; set; }
        public string SaudaBookingDate { get; set; }
        public string SaudaBookingTime { get; set; }
        public string TradeTicketDate { get; set; }
        public string TradeTicketTime { get; set; }
        public string SaudaTTAttachedDate { get; set; }
        public string SaudaTTAttachedTime { get; set; }
        public string SaudaCreationDate { get; set; }
        public string SaudaCreationTime { get; set; }
        public string SaudaReleaseDate { get; set; }
        public string SaudaReleaseTime { get; set; }
        public string TimeGapSaudabookingandrelease { get; set; }
    }
}
