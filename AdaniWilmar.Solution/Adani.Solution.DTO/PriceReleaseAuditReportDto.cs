using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PriceReleaseAuditReportDto
    {
        public DateTime Date { get; set; }
        public string StateName { get; set; }
        public string Vertical { get; set; }
        public string Plant { get; set; }
        public string OilType { get; set; }
        public string MaterialCostUpdateTime { get; set; }
        public string PriceGenerateTime { get; set; }
        public string PriceReleaseTime { get; set; }
        public string TimeGapCostuploadandGenerate { get; set; }
        public string TimeGapCostuploadandRelease { get; set; }
        public string TimegapGenerateandrelease { get; set; }
    }
}
