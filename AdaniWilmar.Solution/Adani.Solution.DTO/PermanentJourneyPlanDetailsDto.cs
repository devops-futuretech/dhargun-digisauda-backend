using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PermanentJourneyPlanDetailsDto
    {
        public long Id { get; set; }
        public long PJPId { get; set; }
        public string RetailerId { get; set; }
        public string Retailers { get; set; }
        public string Retailer { get; set; }
        public long MonthId { get; set; }
        public string Month { get; set; }
        public string NoOfVisit { get; set; }
        public long DistrictId { get; set; }
        public string District { get; set; }
        public long CityId { get; set; }
        public string City { get; set; }
        public long TownId { get; set; }
        public string NoOfDirectDealer { get; set; }
        public string NoOfSubDealer { get; set; }
        public string NoOfWholeSeller { get; set; }
        public long CreatedBy { get; set; }
        public long FinancialYearId { get; set; }
        public string FinancialYear { get; set; }
        public long IsDeleted { get; set; }
        public long StateId { get; set; }
        public string State { get; set; }
        public long TerritoryId { get; set; }
        public string Territory { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

        public string Remarks { get; set; }

        public bool? IsDataChanged { get; set; }

        public int InHQNoVisitId { get; set; }
        public string InHQNoVisitName { get; set; }
    }
}
