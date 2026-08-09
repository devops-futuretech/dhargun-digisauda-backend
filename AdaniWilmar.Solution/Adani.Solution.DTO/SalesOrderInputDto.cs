using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SalesOrderInputDto
    {
        public long SaudaOrderId { get; set; }
        public long DealerId { get; set; }
        public string SaudaNumber { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal QantityInCase { get; set; }
    }
}
