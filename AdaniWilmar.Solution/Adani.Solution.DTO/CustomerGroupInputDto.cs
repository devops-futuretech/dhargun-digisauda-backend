using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CustomerGroupInputDto : KendoGridResult
    {
        public long CustomerGroupId { get; set; }
        public long BDOId { get; set; }
        public bool IsRemoveSelectedDealerIdsFromSession { get; set; }
        public long ZoneId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int TerritoryId { get; set; }

        public List<long> CustomerGroupIds { get; set; }
        public List<long> VerticalIds { get; set; }
        public List<long> ZoneIds { get; set; }
        public List<long> SkuIds { get; set; }
        public List<int> DistrictIds { get; set; }
        public List<int> CityIds { get; set; }
        public List<int> StateIds { get; set; }
        public List<int> TerritoryIds { get; set; }

        public decimal PercentileNumber { get; set; }
    }
}
