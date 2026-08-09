using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaListsDto : IAPIInputDTO
    {
        public int ListCount { get; set; }
        public List<SaudaListDto> SaudaList { get; set; }
        public List<SaudaListGroupDto> SaudaListGroup { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class SkuList 
    {
        public long SkuId { get; set; }    

        public string SkuName { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityInMT { get; set; }
        public decimal PricePercase { get; set; }

    }
    public class SaudaListGroupDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }

        public string DealerCode { get; set; }
        public List<SaudaListDto> SaudaList { get; set; }
        public SaudaListGroupDto()
        {
            SaudaList = new List<SaudaListDto>();
        }
    }
    public class SaudaListDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long SaudaOrderId { get; set; }
        public long DataFilter { get; set; }
        public DateTime BiddingDate { get; set; }
        public decimal TotalBidPrice { get; set; }
        public decimal TotalBidQuantity { get; set; }
        public decimal PendingliftQuantity { get; set; }
        public string TradeTicketNumber { get; set; }
        public string SaudaNumber { get; set; }
        public string User { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string City { get; set; }
        public long CityId { get; set; }
        public bool IsApproved { get; set; }

        public int SaudaStatusId { get; set; }
        public string SaudaStatus { get; set; }
        public string EncryptedId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string Vertical { get; set; }
        public long OilTypeId { get; set; }
        public string OiltypeName { get; set; }
        public string OiltypeCode { get; set; }
        public long SaudaId { get; set; }
        public long DealerId { get; set; }
        public long SkuId { get; set; }

        public decimal QuotedPrice { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }
        public decimal BidPrice { get; set; }

        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }

        public string Incoterms1 { get; set; }
        public string Incoterms2 { get; set; }
        public string PlantName { get; set; }
        public string DealerLocation { get; set; }
        public string SaudaBookingType { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long DiscountTypeId { get; set; }
        //public string Broker { get; set; }
        public string DealerName { get; set; }
        public string CreatedBy { get; set; }

        // NEW: Approval user to display in grid
        public string ApprovalUser { get; set; }


        public DateTime ValidFromDate { get; set; }
        public DateTime ValidToDate { get; set; }
        public string Remarks { get; set; }
        public bool IsActiveRemarks { get; set; }
        public bool IsSAPDataSyncApproval { get; set; }
        public bool IsSaudaApprovalSyncConfirmation { get; set; }
        public bool IsError { get; set; }

        public string DealerCode { get; set; }
        public string BDOName { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StateName { get; set; }
        public bool IsLooseVerticalForAcceptedStatus { get; set; }

        public decimal CounterBidOffer { get; set; }
        public decimal BasePricePerCase { get; set; }
        public decimal BasePricePerSku { get; set; }
        public decimal BidPricePerSku { get; set; }
        public decimal BidPricePerCase { get; set; }
        public long SaudaBookedNumber { get; set; }

        public string BDOCode { get; set; }
        public bool IsSAPDataSync { get; set; }
        public bool IsSapSauda { get; set; }
        public bool IsSapSyncNotReceivedForSaudaNumber { get; set; }
        public bool IsSapSyncNotReceivedForSaudaApprovalConfirmation { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsSaudaApprovalStatusFromSap { get; set; }
        public bool IsSapSaudaNumberUpdateSync { get; set; }
        public decimal NoOfSkusPerCase { get; set; }
        public decimal ForPlantPrice { get; set; }
        public decimal ForDepotPrice { get; set; }
        public decimal ForRakePrice { get; set; }
        public decimal ExPlantPrice { get; set; }
        public decimal ExDepotPrice { get; set; }
        public decimal ExRakePrice { get; set; }
        public long IncotermsTwo { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string SaudaListString { get; set; }
        public string ApproverRemarks { get; set; }
        public List<SkuList> SkuList { get; set; }
        public List<SaudaListDto> SaudaLists { get; set; }
        public int SaudaTypeId { get; set; }
        public string SaudaType { get; set; }
        public string Zones { get; set; }
        public List<long> ZoneIds { get; set; }
        public List<long> StateIds { get; set; }
        public List<long> DistrictIds { get; set; }
        public List<long> CityIds { get; set; }
        public string States { get; set; }
        public string Cities { get; set; }
        public string Districts { get; set; }

        public decimal QPSDiscount { get; set; }

        public long SaudaModificationId { get; set; }
        public decimal PRAmount { get; set; }

        public SaudaListDto()
       {
            SkuList = new List<SkuList>();
            SaudaLists = new List<SaudaListDto>();
       }
    }

    public class SaudaInnerList
    {
        public long Id { get; set; }
        public long SaudaId { get; set; }
        public string SaudaNumber { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string OiltypeName { get; set; }
        public decimal QuotedPrice { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }
        public decimal BidPricePerCase { get; set; }
        public decimal BidPrice { get; set; }
        public DateTime BiddingDate { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidToDate { get; set; }
        public string Incoterms1 { get; set; }
        public long UserId { get; set; }
        public long StatusId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long DiscountTypeId { get; set; }
        public string SaudaBookingType { get; set; }
        public string DiscountType { get; set; }
        public string Status { get; set; }
        public string PlantName { get; set; }
        public string DealerName { get; set; }
        public string BDOName { get; set; }
        public string BDOCode { get; set; }
        public string CreatedBy { get; set; }
        public string DealerCode { get; set; }
        public string StateName { get; set; }
        public bool IsLooseVerticalForAcceptedStatus { get; set; }
        public string Remarks { get; set; }
        public bool IsSAPDataSync { get; set; }
        public bool IsActiveRemarks { get; set; }
        public bool IsSAPDataSyncApproval { get; set; }
        public bool IsSaudaApprovalSyncConfirmation { get; set; }
        public bool IsSapSauda { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsSapSaudaNumberUpdateSync { get; set; }
        public bool IsSaudaApprovalStatusFromSap { get; set; }
        public long SaudaBookedNumber { get; set; }
        public long IncotermsTwo { get; set; }
        public decimal PRAmount { get; set; }
    }

    public class SaudaExportDto : IAPIInputDTO
    {
        public DateTime CreatedDate { get; set; }
        public long Id { get; set; }
        public long SaudaId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public string SaudaNumber { get; set; }
        public DateTime BiddingDate { get; set; }
        public long UserId { get; set; }
        public string DealerName { get; set; }
        public string CreatedBy { get; set; }
        public string SaudaType { get; set; }
        public bool IsSAPDataSync { get; set; }
        public bool IsActiveRemarks { get; set; }
        public bool IsSapSauda { get; set; }
        public bool IsSapSaudaNumberUpdateSync { get; set; }
        public long StatusId { get; set; }
        public long count { get; set; }
        public string Zones { get; set; }
        public string States { get; set; }
        public string Districts { get; set; }
        public string Cities { get; set; }
        public List<SaudaInnerList> InnerList { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public SaudaExportDto()
        {
            
            InnerList = new List<SaudaInnerList>();
        }
    }
    public class ContractNoListDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string SaudaNumber { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal soOpenQuantity { get; set; }
        public long SaudaOrderId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
    }

    public class ContractNoInputDto
    {
        public long SkuId { get; set; }
        public string SaudaNumber { get; set; }
        public long DealerId { get; set; }
        public long SalesOrganizationId { get; set; }
    }

    public class ContractSkuQtyDto
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long SkuUomId { get; set; }
        public string SkuUomName { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal CaseToMetricTonValue { get; set; }
        public decimal MaxAllowableCasesSingleSku { get; set; }
        public decimal MaxAllowableCasesMultipleSku { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal MaximumVehicleCapacityInPercent { get; set; }
        public decimal MaximumVolumeCapacityInPercent { get; set; }
    }

    public class SaudaPendinglistOutputDto
    {
        public long Id { get; set; }
        public long SaudaOrderId { get; set; }
        public long UserId { get; set; }
        public string User { get; set; }
        public string City { get; set; }
        public DateTime BiddingDate { get; set; }
        public decimal TotalBidPrice { get; set; }
        public decimal TotalBidQuantity { get; set; }
        public string OilTypeName { get; set; }
        public long OilTypeId { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public string SaudaNumber { get; set; }        
    }
}
