using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DashboardOverallsaudaOutpuDto
    {
        public long UserId { get; set; }
        public decimal TotalTarget { get; set; }
        public decimal OverallSauda { get; set; }
        public long MonthId { get; set; }
        public string Month { get; set; }
        public List<AchievmentDetailsDto> AchievmentDetailsDto { get; set; }
        public DashboardOverallsaudaOutpuDto()
        {
            AchievmentDetailsDto = new List<AchievmentDetailsDto>();
        }
    }
    public class NewDashboardOverallSaudaOutputDto
    {
        public List<DashboardOverallsaudaOutpuDto> SaudaList { get; set; }
        public decimal TotalTarget { get; set; }
        public decimal OverallSauda { get; set; }
        public QuarterOverallSaudaDto Quarter1 { get; set; }
        public QuarterOverallSaudaDto Quarter2 { get; set; }
        public QuarterOverallSaudaDto Quarter3 { get; set; }
        public QuarterOverallSaudaDto Quarter4 { get; set; }
    }
    public class QuarterOverallSaudaDto
    {
        public decimal TotalTarget { get; set; }
        public decimal OverallSauda { get; set; }
    }

    public class DashboardOverallSalesOutpuDto
    {
        public long UserId { get; set; }
        public decimal TotalTarget { get; set; }
        public decimal OverallSales { get; set; }
        public long MonthId { get; set; }
        public string Month { get; set; }
        public List<AchievmentDetailsDto> AchievmentDetailsDto { get; set; }
        public DashboardOverallSalesOutpuDto()
        {
            AchievmentDetailsDto = new List<AchievmentDetailsDto>();
        }
    }

    public class NewDashboardOverallSalesOutpuDto
    {
        public List<DashboardOverallSalesOutpuDto> SalesList { get; set; }
        public decimal TotalTarget { get; set; }
        public decimal OverallSales { get; set; }
        public QuarterOverallSalesDto Quarter1 { get; set; }
        public QuarterOverallSalesDto Quarter2 { get; set; }
        public QuarterOverallSalesDto Quarter3 { get; set; }
        public QuarterOverallSalesDto Quarter4 { get; set; }
    }
    public class QuarterOverallSalesDto
    {
        public decimal TotalTarget { get; set; }
        public decimal OverallSales { get; set; }
    }
    public class DashboardOverallSalesOutputDto
    {
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public decimal TotalTarget { get; set; }
        public decimal TotalAchievment { get; set; }
        public decimal OverallAchievment { get; set; }
        public decimal OverallTarget { get; set; }
        public long MonthId { get; set; }
        public string Month { get; set; }
        public decimal AchievmentPercentage { get; set; }
    }
    public class NewDashboardOverallSalesOutputDto
    {
        public List<DashboardOverallSalesOutputDto> SalesList { get; set; }
        public decimal TotalTarget { get; set; }
        public decimal OverallSales { get; set; }
        public NewDashboardOverallSalesOutputDto()
        {
            SalesList = new List<DashboardOverallSalesOutputDto>();
        }
    }
    public class OverallPerformanceByUserOutputDto
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Usercode { get; set; }
        public decimal UserTarget { get; set; }
        public decimal UserAchievment { get; set; }
        public decimal AchievmentPercentage { get; set; }
        public int Rank { get; set; }
    }
    public class DashboardDetailsByDealersOutputDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public string TownName { get; set; }
        public decimal Target { get; set; }
        public decimal Achievement { get; set; }
    }
    public class DashboardSaudaDetailsByDealersOutputDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public string TownName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalBookedSaudaValue { get; set; }
        public List<DashboardSaudaDetailsOutputDto> DashboardSaudaDetails { get; set; }
    }
    public class DashboardSaudaDetailsOutputDto
    {
        public long SaudaId { get; set; }
        public string SaudaNumber { get; set; }
        public decimal SaudaBookedQuantity { get; set; }
        public decimal DispatchedQuantity { get; set; }
    }
    public class DashboardSalesDetailsByDealersOutputDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public string TownName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalBookedInvoiceValue { get; set; }
        public bool IsBulkPack { get; set; }
        public IList<DashboardSalesDetailsOutputDto> DashboardSalesDetails { get; set; }
        public DashboardSalesDetailsByDealersOutputDto()
        {
            DashboardSalesDetails = new List<DashboardSalesDetailsOutputDto>();
        }
    }
    public class DashboardSalesDetailsOutputDto
    {
        public long InvoiceId { get; set; }
        public long PackgroupId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal InvoiceValue { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public int SkuId { get; set; }
        public string SkuName { get; set; }
        public bool IsBulkPack { get; set; }
        public long PackGroupId { get; set; }
    }
    public class InvoiceDetailsOutputDto
    {
        public long InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceQuantity { get; set; }
        public decimal TotalInvoiceValue { get; set; }
        public decimal PendingInvoiceValue { get; set; }
        public List<InvoiceSKUDetailsOutputDto> InvoiceSKUDetails { get; set; }
        public InvoiceDetailsOutputDto()
        {
            InvoiceSKUDetails = new List<InvoiceSKUDetailsOutputDto>();
        }
    }
    public class InvoiceSKUDetailsOutputDto
    {
        public long SkuId { get; set; }
        public string sku { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal Quantity { get; set; }
        public decimal QunatityPrice { get; set; }
    }

    public class SalesBilledPartiesDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public decimal TotalTarget { get; set; }
        public decimal TotalAchievment { get; set; }
    }
    public class SalesTourPlanInputDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long LoginUserId { get; set; }
        public long FinancialYearId { get; set; }
    }
    public class SalesTourPlanOutputDto
    {
        public long Month { get; set; }
        public long PlannedVisit { get; set; }
        public long ActualVisit { get; set; }
        public long DeviatedVisit { get; set; }
    }

    public class ZhUserDetailDto
    {
        public long ZonalHeadId { get; set; }
        public string ZonalTrader { get; set; }
        public long DealerId { get; set; }
        public string Dealer { get; set; }
    }


    public class OverallsaudaOutpuForChatBotDto
    {
        public long UserId { get; set; }
        public decimal SaudaBookedForMonth{ get; set; }
        public decimal SaudaBookedForYearly { get; set; }
        public decimal SaudaBookedForQuater { get; set; }

    }


    public class DashboardDetailsForPendingAndOverDueOutputDto
    {
        public decimal TotalBookedValuePendingDue { get; set; }
        public decimal TotalBookedValueOverDue { get; set; }
        public List<OverAndPendingDueWithDealerDetails> OverAndPendingDueWithDealerDetails { get; set; }
        public DashboardDetailsForPendingAndOverDueOutputDto()
        {
            OverAndPendingDueWithDealerDetails = new List<OverAndPendingDueWithDealerDetails>();
        }

    }
    public class OverAndPendingDueWithDealerDetails
    {
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public decimal OverDue { get; set; }
        public decimal PendingDue { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class DashboardSalesOutputDto
    {
        public long DealerId { get; set; }
        public string Dealer { get; set; }
        public string TownName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalBookedInvoiceValue { get; set; }
        public bool IsBulkPack { get; set; }
        public IList<DashboardSalesDetailsOuterListDto> DashboardSalesDetails { get; set; }
        public DashboardSalesOutputDto()
        {
            DashboardSalesDetails = new List<DashboardSalesDetailsOuterListDto>();
        }
    }

    public class DashboardSalesDetailsOuterListDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalInvoiceQuantity { get; set; }
        public decimal TotalInvoiceValue { get; set; }
        public List<InvoiceDetailsInnerListDto> InvoiceList { get; set; }
        public DashboardSalesDetailsOuterListDto()
        {
            InvoiceList = new List<InvoiceDetailsInnerListDto>();
        }
    }

    public class InvoiceDetailsInnerListDto
    {
        public long InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceQuantity { get; set; }
        public decimal InvoiceValue { get; set; }
        public string InvoiceNumber { get; set; }
    }

    public class SalesRegisterDataDto
    {
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal QuantityMT { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }

        public long CreatedBy { get; set; }

    }
    public class SalesRegisterDashboardDto
    {
        public long PackGroupId { get; set; }
        public long CityId { get; set; }
        public decimal QuantityMT { get; set; }
        public string Name { get; set; }
        public string TotalGST { get; set; }
        public long InvoiceId { get; set; }
        public DateTime BillingDate { get; set; }
        public string BillNumber { get; set; }
        public string TotalAmount { get; set; }
    }
    public class SalesRegisterDashDto
    {
        public long PackGroupId { get; set; }
        public decimal QuantityCase { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public DateTime Date { get; set; }
    }

    public class DashboardSauda
    {
        public DateTime Date { get; set; }
        public decimal Achievment { get; set; }
    }

    public class DashboardSalesDto
    {
        public long UserId { get; set; }
        public long PackGroupId { get; set; }
        public decimal QuantityMT { get; set; }
    }
    public class SalesOrderDODetails
    {
        public long DealerId { get; set; }
        public string DealerCode { get; set; }
        public long Id { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public List<string> DeliveryOrderNumbers { get; set; }
        public bool IsCompleted { get; set; }
    }
 }
