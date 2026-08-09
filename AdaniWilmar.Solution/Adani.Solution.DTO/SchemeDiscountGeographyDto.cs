using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SchemeDiscountGeographyDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long SchemeDiscountHistoryId { get; set; }
        public string Name { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> OilPackingTypeIds { get; set; }  /* BPOrCPWise - PackGroup - OilPackingTypeId*/
        public List<long> CustomerGroupIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> SkuIds { get; set; }
        public List<int> CityIds { get; set; }

        public List<long> ZoneIds { get; set; }
        public List<int> StateIds { get; set; }
        public List<int> DistrictIds { get; set; }
        public List<int> TerritoryIds { get; set; }

        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsEdit { get; set; }
        public bool IsActive { get; set; }
        public string Division { get; set; }
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }

        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string Response { get; set; }
        public List<SchemeDiscountGeographyMappingDto> schemeDiscountGeographyDetailsList { get; set; }
        public SchemeDiscountGeographyDto()
        {
            schemeDiscountGeographyDetailsList = new List<SchemeDiscountGeographyMappingDto>();
        }

        public decimal TargetQuantity { get; set; }
        public string DiscountReason { get; set; }

    }
}
