using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class QPSOilTypeListDto
    {
        public long SkuId { get; set; }
        public long OilTypeId { get; set; }
        public long ZoneId { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; }
        public string OilTypeName { get; set; }
        public string ZoneName { get; set; }
        public string SkuCode { get; set; }
        //public string SkuName { get; set; }
    }
}
