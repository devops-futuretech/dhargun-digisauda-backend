using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Adani.Solution.DTO
{
    public class DistributorStockEntrySaveDto
    {
        public long LoginUserId { get; set; }
        public List<DistributorStockSkuInputDto> SkuList { get; set; }
    }

    public class DistributorStockSkuInputDto
    {
        public long SkuId { get; set; }
        public decimal NoOfCases { get; set; }
    }

    public class DistributorStockEntryListOutputDto
    {
        public int ListCount { get; set; }
        public List<DistributorStockEntryDto> StockEntries { get; set; }
    }

    public class DistributorStockEntryDto
    {
        public long EntryId { get; set; }
        public DateTime ReportedDate { get; set; }
        public int SkuCount { get; set; }
        public decimal TotalQuantityInCase { get; set; }
        public decimal TotalQuantityInMT { get; set; }
        public List<DistributorStockSkuDetailDto> SkuDetails { get; set; }
    }

    public class DistributorStockSkuDetailDto
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal QuantityInMT { get; set; }
    }

    public class DealerLatestStockOutputDto
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal QuantityInMT { get; set; }
        public DateTime ReportedDate { get; set; }
    }

    public class DistributorStockReportInputDto
    {
        public long LoginUserId { get; set; }
        public long RoleId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<long> StateIds { get; set; }
        public long VerticalId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long OilTypeId { get; set; }
    }

    public class DistributorStockReportOutputDto
    {
        [DisplayName("Sales Organization")]
        public string SalesOrganization { get; set; }
        [DisplayName("Distribution Channel")]
        public string DistributionChannel { get; set; }
        [DisplayName("Division")]
        public string Division { get; set; }
        [DisplayName("Distributor Name")]
        public string DistributorName { get; set; }
        [DisplayName("Distributor Code")]
        public string DistributorCode { get; set; }
        [DisplayName("Oil Type")]
        public string OilType { get; set; }
        [DisplayName("Material Name")]
        public string MaterialName { get; set; }
        [DisplayName("Material Code")]
        public string MaterialCode { get; set; }
        [DisplayName("Qty in Case")]
        public decimal QtyInCase { get; set; }
        [DisplayName("Qty in MT")]
        public decimal QtyInMT { get; set; }
        [DisplayName("Reported Date and Time")]
        public string ReportedDateTime { get; set; }
    }
}
