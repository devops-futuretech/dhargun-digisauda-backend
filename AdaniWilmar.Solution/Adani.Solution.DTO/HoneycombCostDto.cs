using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HoneycombCostDto
    {
        public long Id { get; set; }
        public long? PlantId { get; set; }
        public string SourcePlantName { get; set; }
        public string SourcePlantCode { get; set; }

        public long? OilTypeId { get; set; }
        public string OilType { get; set; }

        public string ZoneName { get; set; }
        public long ZoneId { get; set; }
        //StateWise
        public int? StateId { get; set; }
        public string StateName { get; set; }
        //public int CityId { get; set; }
        //public string CityName { get; set; }
        //public int DistrictId { get; set; }
        //public string DistrictName { get; set; }
        public long TransportModeId { get; set; }
        public string TransportMode { get; set; }
        public long SkuId { get; set; }
        public List<long> SkuIds { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal CostOrMT { get; set; }
        public long? VerticalId { get; set; }
        public string Vertical { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public long? SubCategoryId { get; set; }
        

        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsPublished { get; set; }

        public decimal RatePerCase { get; set; }
        public long RoleId { get; set; }
    }


    public class KendoGridResultExport : LoginUserIdDto
    {
        public DataSourceRequest DataSourceRequest { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public long IsActiveStatus { get; set; }
    }


    public class SkuUomConversionDto
    {
        public decimal ConversionFactor { get; set; }
        public decimal Quantity { get; set; }
        public decimal LitreConversion { get; set; }
        public long UomId { get; set; }
    }


    public class HoneycombCostExportDto
    {
        public string Vertical { get; set; }
        public string OilType { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
    
        public string Plant { get; set; }
        public string SourceCode { get; set; }

        public string Destination { get; set; }
        public string TransportMode { get; set; }

        public decimal HoneyCombCostOrMT { get; set; }

        public decimal RatePerCase { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Status { get; set; }
        public bool Published { get; set; }

      
    }

}
