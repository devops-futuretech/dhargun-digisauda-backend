using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SurpriseBenefitOutputDto
    {
        public long SaudaOrderId { get; set; }
        public long SaudaId { get; set; }
        public string SaudaNumber { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public long CustomerId { get; set; }
        public long SkuId { get; set; }
        public decimal BidQuantityCase { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidPricePerCase { get; set; }
        public decimal MarginPerCase { get; set; }
    }

    public class SurpriseBenefitPercentileInputDto : KendoGridResult
    {
        public long VerticalId { get; set; }

        public List<long> SkuIds { get; set; }
        public string SkuIdStringList { get; set; }

        public List<long> CustomerIds { get; set; }
        public long PercentileNumber { get; set; }

        public List<long> CustomerGroupIds { get; set; }
        public string CustomerGroupIdStringList { get; set; }

        public List<long> BDOIds { get; set; }
        public string BDOIdStringList { get; set; }

        public List<long> ZonalHeadIds { get; set; }
        public string ZonalHeadList { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }        

        public List<long> ZoneIds { get; set; }
        public string ZoneIdStringList { get; set; }

        public List<long> StateIds { get; set; }
        public string StateIdStringList { get; set; }

        public List<int> TerritoryIds { get; set; }
        public string TerritoryIdStringList { get; set; }

        public List<int> DistrictIds { get; set; }
        public string DistrictIdStringList { get; set; }

        public List<int> CityIds { get; set; }
        public string CityIdStringList { get; set; }
    }
}
