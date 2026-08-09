using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaLimitGroupDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public List<SaudaLimitOutputDto> saudahistory { get; set; }
    }
    public class SaudaLimitOutputDto
    {
        public long Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string LimitRequestNo { get; set; }
        public string Status { get; set; }
        public long StatusId { get; set; }
        public string Remarks { get; set; }
        public decimal RequestQuantityLimit { get; set; }
       
    }
    public class SaudaLimitRequestHistoryDto
    {
        public long Id { get; set; }
        public DateTime RequestDate { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public string LimitRequestNo { get; set; }
        public string Status { get; set; }
        public long StatusId { get; set; }
        public string Remarks { get; set; }
        public decimal RequestQuantityLimit { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public decimal ActualLimit { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
