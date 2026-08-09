using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class GstDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long? PlantId { get; set; }
        public string PlantName { get; set; }

        public bool IsActive { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public long DepotId { get; set; }
        public string DepotName { get; set; }
        public string DepotCode { get; set; }

        public long? SourceZoneId { get; set; }
        public string SourceZoneName { get; set; }
        public int? SourceStateId { get; set; }
        public string SourceStateName { get; set; }

        public long? DestinationZoneId { get; set; }
        public string DestinationZoneName { get; set; }
        public int? DestinationStateId { get; set; }
        public string DestinationStateName { get; set; }

        public long? FreightZoneId { get; set; }
        public string FreightZoneName { get; set; }

        public long FreightRouteId { get; set; }
        public string FreightRouteName { get; set; }

        public long? VerticalId { get; set; }
        public string Vertical { get; set; }
        //OilWise
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        //BPOrCPWise
        public long OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }

        public long SkuId { get; set; }
        public List<long> SkuIds { get; set; }

        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long SubCategoryId { get; set; }
        public string SubCategory { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class GSTUploadOldDto : CommonResultDto
    {
        public string SourceZone { get; set; }
        public string SourceState { get; set; }
        public string PlantName { get; set; }
        public string DestinationZone { get; set; }
        public string DestinationState { get; set; }
        public string FreightZoneName { get; set; }
        public string FreightRouteName { get; set; }
        public string VerticalCode { get; set; }
        public string OilTypeName { get; set; }
        public string SkuCode { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int CreatedBy { get; set; }
    }

    public class GSTUploadDto : CommonResultDto
    {
        public string SourceState { get; set; }
        public string DestinationState { get; set; }
        public string PlantName { get; set; }
        public string OilTypeName { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int CreatedBy { get; set; }
        public long ParentId { get; set; }
    }

    public class GstInputDto : IAPIInputDTO
    {
        public long Id { get; set; }

        public List<int> SourceStateIds { get; set; }

        public List<int> DestinationStateIds { get; set; }

        public List<long> DepotIds { get; set; }

        public List<long> OilTypeIds { get; set; }

        public long VerticalId { get; set; }

        public bool IsActive { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public decimal CGST { get; set; }

        public decimal SGST { get; set; }

        public decimal IGST { get; set; }

        public long LoginUserId { get; set; }

        public bool PostStatus { get; set; }

        public string PostMessage { get; set; }

    }

    public class GstUpdateDto
    {
        public long DepotId { get; set; }
        public long OilTypeId { get; set; }
        public int SourceStateId { get; set; }
        public int DestinationStateId { get; set; }
    }

    public class ExportGstDto
    {
        public string SourceState { get; set; }
        public string DestinationState { get; set; }
        public string DepotName { get; set; }
        public string OilType { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string IsActive { get; set; }
    }   
}

