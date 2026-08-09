using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class SalesTourPlanPcpHistory : Auditable
    {
        public long FinancialYearId { get; set; }
        public int StateId { get; set; }
        public int TerritoryId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public string NoOfDirectDealer { get; set; }
        public string NoofSubDealer { get; set; }
        public string NoOfWholeSeller { get; set; }
        public long NoOfVisit { get; set; }
        public long PermanentJourneyPlanDetailId { get; set; }
        public long DealerId { get; set; }
        public bool IsDataChanged { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EffectiveFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime EffectiveTo { get; set; }

        public int InHQNoVisit { get; set; }

        public string Remarks { get; set; }

        public virtual FinancialYear FinancialYear { get; set; }
        //public virtual State State { get; set; }
        //public virtual Territory Territory { get; set; }
        //public virtual District District { get; set; }
        //public virtual City City { get; set; }
        //public virtual User Dealer { get; set; }
    }
}
