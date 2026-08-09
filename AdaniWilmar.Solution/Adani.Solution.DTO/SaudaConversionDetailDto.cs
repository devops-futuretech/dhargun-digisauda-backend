using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaConversionDetailDto
    {
        public long SaudaConversionId { get; set; }
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalQuantityCase { get; set; }
        public DateTime ConversionDate { get; set; }
        public DateTime BookedDate { get; set; }
        public DateTime? ExtendToDate { get; set; }
        public string Remarks { get; set; }
        public IList<SaudaOrderDetails> SaudaConversionOrders { get; set; }
        public SaudaConversionDetailDto()
        {
            SaudaConversionOrders = new List<SaudaOrderDetails>();
        }
    }


    public class SaudaConversionDetailForAdminDto : IAPIInputDTO
    {
        public SaudaConversionDetailForAdminDto()
        {
            SaudaConversionOrders = new List<SaudaOrderDetails>();
        }

        public long SaudaConversionId { get; set; }
        public long SaudaId { get; set; }
        public string SaudaNumber { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalQuantityCase { get; set; }
        public DateTime ConversionDate { get; set; }
        public long? StatusId { get; set; }
        public string StatusName { get; set; }
        public bool IsConversion { get; set; }
        public bool IsExtension { get; set; }
        public IList<SaudaOrderDetails> SaudaConversionOrders { get; set; }

        public string ApproverRemarks { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ExtendToDate { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }     
    }


    public class SaudaConversionDetailsForEmail
    {
        public decimal QuantityInMt { get; set; }
        public decimal QuantityInSku { get; set; }
        public decimal BaseRate { get; set; }
        public long SaudaNumber { get; set; }
        public long SkuCode { get; set; }
        public string SkuName { get; set; }
        public long DealerId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public long PlantCode { get; set; }
        public long DepotCode { get; set; }
        public string PlantOrDepotCode { get; set; }
        public string SkuCodeInString { get; set; }
        public string Remarks { get; set; }
    }

}
