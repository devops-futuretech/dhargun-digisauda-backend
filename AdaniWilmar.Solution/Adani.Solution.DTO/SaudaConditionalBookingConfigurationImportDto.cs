using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConditionalBookingConfigurationImportDto
    {
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public string StateName { get; set; }
        public string ZoneName { get; set; }
        public string OilTypeName { get; set; }
        public string PackGroup { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EssentialSkuCode { get; set; }
        public string MandatorySkuCode { get; set; }
        public string MandatorySkuPercentage { get; set; }
        public string Message { get; set; }
        public long LoginUserId { get; set; }

    }
}
