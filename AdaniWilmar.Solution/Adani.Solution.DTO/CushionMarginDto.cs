using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CushionMarginDto
    {
        public long Id { get; set; }

        public long? SalesOrganizationId { get; set; }
        public string SalesOrganization { get; set; }

        public long? DistributionChannelId { get; set; }
        public string DistributionChannel { get; set; }

        public long? VerticalId { get; set; }
        public string Vertical { get; set; }
        //OilWise
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
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
        public long RoleId { get; set; }
    }
}
