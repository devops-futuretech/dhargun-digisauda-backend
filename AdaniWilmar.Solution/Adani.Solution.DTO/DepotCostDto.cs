using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DepotCostDto
    {
        public long Id { get; set; }
        public long DepotId { get; set; }
        public string DepotName { get; set; }
        public string DepotCode { get; set; }
        public decimal CostOrMT { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsPublished { get; set; }
        public long RoleId { get; set; }

        public long? SkuId { get; set; }
        public List<long> SkuIds { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public long? OilTypeId { get; set; }
        public string OilType { get; set; }

        //BPOrCPWise - Packgroup
        public long? OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }


    }

    public class OilTransferCostDto
    {
        public long Id { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SourceId { get; set; }
        public string SourceName { get; set; }
        public string DepotCode { get; set; }
        public decimal CostOrMT { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsPublished { get; set; }
        public long DestinationId { get; set; }
        public string DestinationName { get; set; }

        public long OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }

        public List<long> SkuIds { get; set; }
        public string SkuName { get; set; }
        public long RoleId { get; set; }
    }

    public class AdditionalCostDto
    {
        public long Id { get; set; }
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public decimal CostOrMT { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsPublished { get; set; }
        public long PlantId { get; set; }
        public string PlantName { get; set; }
        public long RoleId { get; set; }

    }
}
