using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PermanentCoveragePlanReportInputDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long VerticalId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public List<long> BDOIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
    }

    public class PermanentCoveragePlanReportOutputDto
    {

        public long FinancialYearId { get; set; }
        public string Year { get; set; }
        public string DealerId { get; set; }
        public string ZonalHeadName { get; set; }
        public string BDOName { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

        public string PCPNumber { get; set; }
        public DateTime CreatedDate { get; set; }

        public long HeadquartersId { get; set; }
        public string Headquarters { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }

        public int TerritoryId { get; set; }
        public string Territory { get; set; }

        public int DistrictId { get; set; }
        public string District { get; set; }

        public long CityId { get; set; }
        public string City { get; set; }

        public string Dealer { get; set; }

        public string NoOfSubDealer { get; set; }
        public string NoOfWholeSeller { get; set; }
        public decimal NoOfVisit { get; set; }

        public int InHQNoVisitId { get; set; }
        public string InHQNoVisitName { get; set; }

        public string Remarks { get; set; }
    }

    public class PCPExport
    {
        [DisplayName("PCP Number")]
        public string PCPNumber { get; set; }
        [DisplayName("Created Date")]
        public string CreatedDate { get; set; }
        [DisplayName("Zonal Head Name")]
        public string ZonalHeadname { get; set; }
        [DisplayName("State Trader Name")]
        public string StateTraderName { get; set; }
        [DisplayName("Year")]
        public string Year { get; set; }
        [DisplayName("Effective From")]
        public string EffectiveFrom { get; set; }
        [DisplayName("Effective To")]
        public string EffectiveTo { get; set; }
        [DisplayName("State")]
        public string State { get; set; }
        [DisplayName("District")]
        public string District { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [DisplayName("Distributor")]
        public string Dealer { get; set; }
        [DisplayName("No Of Sub Dealer")]
        public string NoOfSubDealer { get; set; }
        [DisplayName("No Of Whole Seller")]
        public string NoOfWholeSeller { get; set; }
        [DisplayName("Number of Visit")]
        public string NoOfVisit { get; set; }
        [DisplayName("In HQ/No Visit")]
        public string InHQVisit { get; set; }
        [DisplayName("Remarks")]
        public string Remarks { get; set; }
    }

    public class ReportSelectDto
    {
        public long ZonalHeadId { get; set; }
        public long BDOId { get; set; }
    }
}


