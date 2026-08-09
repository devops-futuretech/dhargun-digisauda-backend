using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class RaMarginDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long? VerticalId { get; set; }
        //public string Vertical { get; set; }

        //OilWise
        public long OilTypeId { get; set; }
        //public string OilType { get; set; }
        //BPOrCPWise

        public long OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }

        //StateWise
        public int StateId { get; set; }
        public string StateName { get; set; }

        public int? TerritoryId { get; set; }
        public string TerritoryName { get; set; }

        public int? CityId { get; set; }
        public string CityName { get; set; }

        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }

        public decimal RatePerMt { get; set; }
        public string CustomerCategoryWise { get; set; }

        public long? SkuId { get; set; }
        public List<long> SkuIds { get; set; }

        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string ZoneName { get; set; }
        public long ZoneId { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }

        public long? SubCategoryId { get; set; }
        public bool IsPublished { get; set; }

        public decimal RatePerCase { get; set; }

        public string VerticalName { get; set; }
        public string OilTypeName { get; set; }
        public long RoleId { get; set; }
    }
}
