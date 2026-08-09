using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    class SchemeGeographyReportDto { }

    public class SchemeGeographyListOutputDto
    {
        public int ListCount { get; set; }
        public List<SchemeGeographyReportOutputDto> SchemeGeographyReportOutput { get; set; }

    }
    public class SchemeGeographyReportInputputDto
    {
        public List<long> StateIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string StateId { get; set; }
        public long VerticalId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public int SaudaBookingTypeId { get; set; }
        public List<long> BDOIds { get; set; }
        public int PackTypeId { get; set; }
        public int StatusId { get; set; }
        public List<long> GeographySchemeIds { get; set; }
        public int PageNo { get; set; }
    }

    public class SchemeGeographyReportOutputDto
    {
        //String
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string BDOCode { get; set; }
        public string BDOName { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string SchemeID { get; set; }
        public string SchemeName { get; set; }


        //Decimal
        public decimal TargetQuantity { get; set; }
        public decimal AchievedQuantity { get; set; }
        public decimal Progress { get; set; }

        //DateTime
        public DateTime? AchievedDate { get; set; }

        public long Ranking { get; set; }


    }

    public class SchemeGeographyReportExportDto
    {
        [DisplayName("Scheme Name")]
        public string SchemeName { get; set; }
        [DisplayName("Material Code")]
        public string SkuCode { get; set; }
        [DisplayName("Material Name")]
        public string SkuName { get; set; }
        [DisplayName("Distributor Code")]
        public string DealerCode { get; set; }
        [DisplayName("Distributor Name")]
        public string DealerName { get; set; }
        [DisplayName("State Trader Name")]
        public string Bdoname { get; set; }
        [DisplayName("Target Quantity")]
        public string TargetQuantity { get; set; }
        [DisplayName("Target Achieved")]
        public string TargetAchieved { get; set; }
        [DisplayName("Date Achieved")]
        public string DateAchieved { get; set; }
        [DisplayName("Progress %")]
        public string Progress { get; set; }
        public string Ranking { get; set; }


    }
}
