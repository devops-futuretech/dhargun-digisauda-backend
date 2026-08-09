using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaLimitHistoryDto
    {
        public long Id { get; set; }
        public long DealerId { get; set; }
        public string Remarks { get; set; }
        public decimal OldSaudaLimit { get; set; }
        public decimal NewSaudaLimit { get; set; }
        public long LoginUserId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
    }
}
