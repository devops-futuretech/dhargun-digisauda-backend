using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthlyTourPlanReportInputDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long VerticalId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public List<long> BDOIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
    }

    public class MonthlyTourPlanOutputDto
    {

        public long Id { get; set; }
        public long MTPId { get; set; }
        public string DealerId { get; set; }
        public string ZonalHeadName { get; set; }
        public string BDOName { get; set; }
        public DateTime Date { get; set; }

        public DateTime CreatedDate { get; set; }
        public string MTPNumber { get; set; }

        public long HeadquartersId { get; set; }
        public string Headquarters { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }

        public int TerritoryId { get; set; }
        public string Territory { get; set; }

        public int DistrictId { get; set; }
        public string District { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

        public string Area { get; set; }

        public string Dealer { get; set; }

        public string NoOfSubDealer { get; set; }
        public string NoOfWholeSeller { get; set; }
        public string NoOfVisit { get; set; }

        public int InHQNoVisitId { get; set; }
        public string InHQNoVisitName { get; set; }

        public string Remarks { get; set; }
    }

    public class MonthlyTourPlanExportDto
    {
        [DisplayName("MTP Number")]
        public string MTPNumber { get; set; }
        [DisplayName("Created Date")]
        public string CreatedDate { get; set; }
        [DisplayName("Zonal Head Name")]
        public string ZonalHeadName { get; set; }
        [DisplayName("State Trader name")]
        public string StateTraderName { get; set; }
        [DisplayName("Date")]
        public string Date { get; set; }
        [DisplayName("Day")]
        public string Day { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [DisplayName("Area")]
        public string Area { get; set; }
        [DisplayName("Distributor")]
        public string Distributor { get; set; }
        [DisplayName("Remarks")]
        public string Remarks { get; set; }
        [DisplayName("In HQ/No Visit")]
        public string InHQVisit { get; set; }
    }

}

