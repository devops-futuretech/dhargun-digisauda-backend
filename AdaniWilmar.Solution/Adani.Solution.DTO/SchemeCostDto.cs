using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SchemeCostDto
    {
        public long Id { get; set; }
        public long? VerticalId { get; set; }
        public string Vertical { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public decimal CostOrMt { get; set; }
        public string ZoneName { get; set; }
        public long ZoneId { get; set; }        
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int? TerritoryId { get; set; }
        public string TerritoryName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }   
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public long OilPackingTypeId { get; set; }
        public string OilPackingTypeName { get; set; }
        public bool IsPublished { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public long? SubCategoryId { get; set; }
        public List<long> SkuIds { get; set; }
        public long RoleId { get; set; }
    }
}
