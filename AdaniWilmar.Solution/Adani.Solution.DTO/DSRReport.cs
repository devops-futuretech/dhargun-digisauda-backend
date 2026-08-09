using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DSRReportDTO
    {
        public DateTime Date { get; set; }

        public string DealerName { get; set; }

        public string PendingSaudaNO { get; set; }

        public string PendingSaudaNORemarks { get; set; }

        public string MarketScenarioTitle { get; set; }

        public string MarketScenarioRemarks { get; set; }

        public string CompetitorName { get; set; }

        public string ProductName { get; set; }

        public decimal Quantity { get; set; }

        public decimal Rate { get; set; }

        public string BDOName { get; set; }

        public string WholeSellerName { get; set; }

        public string OilType { get; set; }

        public string SkuName { get; set; }

        public decimal QtyperCase { get; set; }

        public decimal Price { get; set; }

        public string ProspectName { get; set; }

        public long MobileNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public decimal ProspectiveSales { get; set; }

        public decimal ProspectiveInterestLevel { get; set; }

        public decimal BusinessPotentialPeryear { get; set; }
    }
    public class DSRReportInputdto
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<long> ZHIds { get; set; }
        public List<long> BDOIds { get; set; }

        public long ReportType { get; set; }
        public long VerticalId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
    }


    public class DSRDealerVisitDto
    {
        public string Date { get; set; }
        [DisplayName("Distributor Name")]
        public string DistributorName { get; set; }
        [DisplayName("Pending Sauda Number")]
        public string PendingSaudaNumber { get; set; }
        [DisplayName("Pending Sauda Number Remarks")]
        public string PendingSaudaRemarks { get; set; }
        [DisplayName("Market Scenario Title")]
        public string MarketScenario { get; set; }
        [DisplayName("Market Scenario Remarks")]
        public string MarketScenarioRemarks { get; set; }
        [DisplayName("Competitor Name")]
        public string CompetitorName { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
    }

    public class DSRWholeSalerReport
    {
        public string Date { get; set; }
        [DisplayName("State Trader Name")]
        public string StateTrader { get; set; }
        [DisplayName("Distributor Name")]
        public string DealerName { get; set; }
        [DisplayName("Whole Saler Name")]
        public string WholeSalerName { get; set; }
        public string OilType { get; set; }
        [DisplayName("Material Name")]
        public string Skuname { get; set; }
        [DisplayName("Quantity Per MT")]
        public string QuantityMT { get; set; }
        public string Price { get; set; }
    }

    public class ProspectiveDealerExport
    {
        [DisplayName("Date")]
        public string Date { get; set; }
        [DisplayName("Prospect name")]
        public string ProspectName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        [DisplayName("Prospective Sales")]
        public string ProspectiveSales { get; set; }
        [DisplayName("Prospective InterestLevel")]
        public string ProspectiveIntrestLevel { get; set; }
        [DisplayName("Business Potential Peryear")]
        public string BusinessPotentialyear { get; set; }
    }
    }
