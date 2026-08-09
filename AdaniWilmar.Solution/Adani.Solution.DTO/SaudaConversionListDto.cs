using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaConversionListDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public DateTime ConversionDate { get; set; }
        public long? StatusId { get; set; }
        public string StatusName { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ExtendToDate { get; set; }
        public long Id { get; set; }
        public long SaudaId { get; set; }
        public bool IsConversion { get; set; }
        public bool IsExtension { get; set; }
        public string SaudaNumber { get; set; }
        public string CityName { get; set; }
        public string PlantName { get; set; }
        public string IncoTerm { get; set; }
        public IList<SaudaOrderDetails> SaudaOrderDetailsList { get; set; }

        public SaudaConversionListDto()
        {
            SaudaOrderDetailsList = new List<SaudaOrderDetails>();
        }
    }

    public class SaudaConvertionFilterDto : UserIdDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long StatusId { get; set; }
        public long VerticalId { get; set; }
    }

    public class SaudaConversionWithOrderDetailListDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public DateTime ConversionDate { get; set; }
        public long? StatusId { get; set; }
        public string StatusName { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ExtendToDate { get; set; }
        public long Id { get; set; }
        public long SaudaId { get; set; }
        public bool IsConversion { get; set; }
        public bool IsExtension { get; set; }
        public string SaudaNumber { get; set; }
        public string CityName { get; set; }
        public string PlantName { get; set; }
        public string IncoTerm { get; set; }

        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCases { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidPricePerCase { get; set; }
        public decimal QuotedPrice { get; set; }
        public decimal PendingQuantity { get; set; }
        public decimal PendingQuantityCases { get; set; }
    }

    public class SaudaExtensionFilterDto : UserIdDto
    {
        public List<long?> OilTypeIds { get; set; }
        public List<long> BdoIds { get; set; }
        public List<long> DealerIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }

    public class SaudaExtensionFilterDtoForGrid : KendoGridResult
    {
         public long statusId { get; set; }
         public DateTime ValidFrom { get; set; }
         public DateTime ValidTo { get; set; }
    }

    public class ChequeStatusReportInputDto 
    {
        public long ZonalHeadId { get; set; }
        public List<long> BdoIds { get; set; }
        public List<long> DealerIds { get; set; }
    }

    public class ChequeStatusReportOutputDto
    {
        public string ChequeNo { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public string BdoName { get; set; }
        public string BdoCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }


}
