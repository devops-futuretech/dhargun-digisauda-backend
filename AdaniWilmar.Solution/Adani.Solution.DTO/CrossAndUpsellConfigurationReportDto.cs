using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CrossAndUpsellConfigurationReportDto
    {
        public long Id { get; set; }
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        public string OilType { get; set; }
        public string PackGroup { get; set; }
        public string Zone { get; set; }
        public string State { get; set; }
        public string EssentialSku { get; set; }
        public string MandatorySku { get; set; }
        public decimal MandatorySkuPercentage { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
