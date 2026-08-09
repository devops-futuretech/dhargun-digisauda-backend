using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class ConfigurationDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Type { get; set; }
        public int SaudaBookingTypeId { get; set; }
        public bool IsNotification { get; set; }
    }

    public class SaudaValidityAndSaudaReportMailConfigurationDto 
    {
        public long Id { get; set; }
        public long VerticalId { get; set; }
        public long RoleId { get; set; }
        public string EmailIds { get; set; }
        public List<long> VerticalsBasedOnSaudaValidity { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
    }
