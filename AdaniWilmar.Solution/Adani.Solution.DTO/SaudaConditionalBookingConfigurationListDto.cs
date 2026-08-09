using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConditionalBookingConfigurationListDto
    {
        public long Id { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganizationName { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannelName { get; set; }
        public long DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string ZoneId { get; set; }
        public string ZoneNames { get; set; }
        public string StateId { get; set; }
        public string StateNames { get; set; }
        public string OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long PackGroupId { get; set; }
        public string PackGroupName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string EncryptedId { get; set; }
        public long LoginUserId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
