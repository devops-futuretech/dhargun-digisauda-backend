using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SchemeDiscountUserMappingDto : KendoGridResult
    {
        public long SchemeDiscountUserMappingId { get; set; }
        public long SchemeDiscountUserId { get; set; }
        public long CustomerGroupId { get; set; }
        public string CustomerGroup { get; set; }

        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public string PackGroup { get; set; }

        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public bool IsActive { get; set; }
    }
}
