using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MTPVsDSRDeviationReportDto
    {
        public long Sno { get; set; }
        public long BDOId  { get; set; }
        public string BDOName { get; set; }
        public string CityName { get; set; }
        public decimal Month1PlannedVisitCount { get; set; }
        public decimal Month1ActualVisitCount { get; set; }
        public decimal Month2PlannedVisitCount { get; set; }
        public decimal Month2ActualVisitCount { get; set; }
        public decimal Month3PlannedVisitCount { get; set; }
        public decimal Month3ActualVisitCount { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }

    public class MTPVsDSRDeviationExportDto
    {
        [DisplayName("S.NO")]
        public long Sno { get; set; }
        [DisplayName("State Trader Name")]
        public string BDOName { get; set; }
        [DisplayName("City Name")]
        public string CityName { get; set; }
        [DisplayName("Planned Visit Count for Month -1")]
        public decimal Month1PlannedVisitCount { get; set; }
        [DisplayName("Actual Visit Count for Month -1")]
        public decimal Month1ActualVisitCount { get; set; }
        [DisplayName("Planned Visit Count for Month -2")]
        public decimal Month2PlannedVisitCount { get; set; }
        [DisplayName("Actual Visit Count for Month -2")]
        public decimal Month2ActualVisitCount { get; set; }
        [DisplayName("Planned Visit Count for Month -3")]
        public decimal Month3PlannedVisitCount { get; set; }
        [DisplayName("Actual Visit Count for Month -3")]
        public decimal Month3ActualVisitCount { get; set; }
        
    }
}
