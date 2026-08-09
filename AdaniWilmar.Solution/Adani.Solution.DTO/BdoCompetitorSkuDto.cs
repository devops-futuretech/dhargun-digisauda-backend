using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class BdoCompetitorSkuDto
    {
        public long BdoCompetitorId { get; set; }
        public string SkuName { get; set; }
        public decimal QuanityPerMt { get; set; }
        public decimal Price { get; set; }
        public long CreatedBy { get; set; }
    }
}
