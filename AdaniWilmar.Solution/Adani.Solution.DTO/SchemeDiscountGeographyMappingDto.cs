using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SchemeDiscountGeographyMappingDto : KendoGridResult
    {
        public long SchemeDiscountGeographyMappingId { get; set; }
        public long SchemeDiscountId { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public long CustomerGroupId { get; set; }
        public string CustomerGroup { get; set; }

        public long OilPackingTypeId { get; set; }  /* BPOrCPWise - PackGroup - OilPackingTypeId*/
        public string OilPackingType { get; set; }

        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public string OilTypeCode { get; set; }
        public string PackGroup { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; }

        public long DistrictId { get; set; }
        public string DistrictName { get; set; }

        public long CityId { get; set; }
        public string CityName { get; set; }

        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }

        public bool IsActive { get; set; }
    }
}
