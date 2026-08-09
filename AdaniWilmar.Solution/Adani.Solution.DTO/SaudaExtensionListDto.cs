using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaExtensionListDto
    {
        public long SaudaConversionId { get; set; }
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ExtendToDate { get; set; }
        public long? StatusId { get; set; }
        public string StatusName { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
    }

    public class SaudaBookedListDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public List<SaudaBookedSaudaWithExtensionDetailsListDto> SaudaBookedList { get; set; }

        public SaudaBookedListDto()
        {
            SaudaBookedList = new List<SaudaBookedSaudaWithExtensionDetailsListDto>();
        }

    }
    public class SaudaBookedSaudaWithExtensionDetailsListDto
    {
        public long SaudaOrderId { get; set; }
        public long PendingContractId { get; set; }
        public string SaudaNumber { get; set; }
        public DateTime? SaudaBookedDate { get; set; }
        public DateTime? SaudaValidToDate { get; set; }
        public DateTime? SaudaExtendedToDate { get; set; }
        public decimal SaudaQuantityMT { get; set; }
        public decimal SaudaQuantityCase { get; set; }
        public string BookedSku { get; set; }
        public string SaudaExtendedDays { get; set; }
        public string SaudaRequestDate { get; set; }
        public decimal BasicRate { get; set; }
        public string DealerName { get; set; }
        public decimal SaudaQuantityInMt { get; set; }
        public DateTime? SaudaValidFromDate { get; set; }
        public string Remarks { get; set; }
        public string SAPRemarks { get; set; }
        public bool IsApproval { get; set; }
        public string BdoName { get; set; }
        public long BdoId { get; set; }
        public long DealerId { get; set; }
        public string BdoAddress { get; set; }
        public string DealerAddress { get; set; }
        public string zonalHeadName { get; set; }
        public decimal PendingQuantityMT { get; set; }
        public decimal PendingQuantityCase { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public long Id { get; set; }
        public bool SaudaExtensionUpdateFromSap { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsSapDataSync { get; set; }
        public long DivisionId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public string SkuCode { get; set; }

        public List<SaudaExtensionSkuListDto> SkuList { get; set; }

        public SaudaBookedSaudaWithExtensionDetailsListDto()
        {
            SkuList = new List<SaudaExtensionSkuListDto>();
        }
    }
    public class SaudaExtensionSkuListDto
    {
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long SkuId { get; set; }
    }

    public class SaudaExtensionPendingAndApprovedListDto
    {
        public List<SaudaBookedSaudaWithExtensionDetailsListDto> PendingList { get; set; }
        public List<SaudaBookedSaudaWithExtensionDetailsListDto> ApprovedList { get; set; }
        public SaudaExtensionPendingAndApprovedListDto()
        {
            PendingList = new List<SaudaBookedSaudaWithExtensionDetailsListDto>();
            ApprovedList = new List<SaudaBookedSaudaWithExtensionDetailsListDto>();
        }
    }

    public class SaudaExtensionPendingAndApprovedDto
    {
        public List<SaudaBookedListDto> PendingList { get; set; }
        public List<SaudaBookedListDto> ApprovedList { get; set; }
        public SaudaExtensionPendingAndApprovedDto()
        {
            PendingList = new List<SaudaBookedListDto>();
            ApprovedList = new List<SaudaBookedListDto>();
        }
    }

    public class SaudaBookedSaudaWithExtensionDetailsExportDto
    {
        public string SaudaNumber { get; set; }
        public string ZonalTrader { get; set; }
        public string StateTrader { get; set; }
        public string Dealer { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string ExtendedDays { get; set; }
        public string ExtendedDate { get; set; }
        public string BookedSku { get; set; }
        public string SaudaQuantityMT { get; set; }
        public string SaudaQuantityCase { get; set; }
        public string PendingQuantityMT { get; set; }
        public string PendingQuantityCase { get; set; }
        public decimal BaseRate { get; set; }
        public string Remarks { get; set; }
        public string SAPRemarks { get; set; }
        public long Id { get; set; }
        public bool SaudaExtensionUpdateFromSap { get; set; }
        public bool IsApproval { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsSapDataSync { get; set; }
    }
   }
