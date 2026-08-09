using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SurpriseBenefitDownloadInputDto
    {
        //public long CustomerGroupId { get; set; }
        public string CustomerGroupIds { get; set; }
        public string BDOIds { get; set; }
        public string ZonalHeadIds { get; set; }
        public string SkuIds { get; set; }
        public string CustomerIds { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int PercentileNumber { get; set; }
        public string CityIds { get; set; }

        public string TerritoryIds{ get; set; }
        public string DistrictIds { get; set; }
        public long VerticalId { get; set; }
    }
}
