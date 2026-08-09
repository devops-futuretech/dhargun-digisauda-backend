using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TargetVsAchievementReportDto
    {
        public string StateName { get; set; }
        public string ZonalTrader { get; set; }
        public string BDOKAM { get; set; }
        public decimal Target { get; set; }
        public decimal Achievement { get; set; }
        public decimal AchievementPercentage { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
    public class TargetVsAchievementExportDto
    {
        [DisplayName("State")]
        public string StateName { get; set; }
        [DisplayName("Zonal Trader")]
        public string ZonalTrader { get; set; }
        [DisplayName("StateTrader/KAM")]
        public string BDOKAM { get; set; }
        public decimal Target { get; set; }
        public decimal Achievement { get; set; }
        [DisplayName("Achievement %")]
        public decimal AchievementPercentage { get; set; }
       
    }
}
