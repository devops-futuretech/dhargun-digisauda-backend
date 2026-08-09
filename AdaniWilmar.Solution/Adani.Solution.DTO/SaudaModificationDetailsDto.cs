using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaModificationDetailsDto
    {
        public long Id { get; set; }
        public string SaudaNumber { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public List<SaudaModificationDetailLineDto> Lines { get; set; }

        public SaudaModificationDetailsDto()
        {
            Lines = new List<SaudaModificationDetailLineDto>();
        }
    }

    public class SaudaModificationDetailLineDto
    {
        public long Id { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long OilPackGroupTypeId { get; set; }
        public string OilPackGroupTypeName { get; set; }
        public decimal TotalOriginalPendingQty { get; set; }
        public decimal TotalModifiedQty { get; set; }
        public List<SaudaModificationDetailItemDto> NewItems { get; set; }
        public List<SaudaModificationDetailItemDto> OldItems { get; set; }

        public SaudaModificationDetailLineDto()
        {
            NewItems = new List<SaudaModificationDetailItemDto>();
            OldItems = new List<SaudaModificationDetailItemDto>();
        }
    }

    public class SaudaModificationDetailItemDto
    {
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal SaudaQuantity { get; set; }
    }

    public class SaudaModificationsListsDto : IAPIInputDTO
    {
        public int ListCount { get; set; }
        public List<SaudaModificationDetailLineDto> SaudaModificationsList { get; set; }
        public List<SaudaModificationNewItemDto> SaudaModificationNewItemsList { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class SaudaModificationNewItemDto
    {
        public string OilTypeName { get; set; }
        public string OilPackGroupTypeName { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public decimal QuantityInCase { get; set; }
        public decimal QuantityInMT { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}


