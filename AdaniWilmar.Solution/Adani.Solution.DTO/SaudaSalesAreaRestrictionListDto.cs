using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaSalesAreaRestrictionListDto
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public TimeSpan TimeRestriction { get; set; }
        public string TimeRestrictionString { get { return TimeRestriction.ToString(@"hh\:mm"); } }
        public bool IsActive { get; set; }

        public long DivisionId { get; set; }
        public string DivisionName { get; set; }

        public long SalesOrganizationId { get; set; }
        public string SalesOrganizationName { get; set; }

        public long DistributionChannelId { get; set; }
        public string DistributionChannelName { get; set; }
    }

    public class SaudaSalesAreaRestrictionDto
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public TimeSpan TimeRestriction { get; set; }
        public string TimeRestrictionString { get { return TimeRestriction.ToString(@"hh\:mm"); } }
        public bool IsActive { get; set; }

        public long DivisionId { get; set; }
        public string DivisionName { get; set; }

        public long SalesOrganizationId { get; set; }
        public string SalesOrganizationName { get; set; }

        public long DistributionChannelId { get; set; }
        public string DistributionChannelName { get; set; }

        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class SaudaSalesAreaRestrictionConfigurationExportDto
    {
        public long Id { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string TimeRestrictionString { get; set; }
        public string SalesOrganizationName { get; set; }
        public string DistributionChannelName { get; set; }
        public string DivisionName { get; set; }
    }
}
