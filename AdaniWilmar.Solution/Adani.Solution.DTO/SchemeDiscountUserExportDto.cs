using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SchemeDiscountUserExportDto
    {
        public string SchemeName { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string CustomerGroup { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string PackGroup { get; set; }
        public string OilTypeName { get; set; }
        public decimal Discount { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public bool IsActive { get; set; }        
    }
}
