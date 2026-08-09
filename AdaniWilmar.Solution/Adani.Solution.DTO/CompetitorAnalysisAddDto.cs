using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CompetitorAnalysisAddDto
    {
        public long Id { get; set; }
        public long SkuId { get; set; }
        public long? OilTypeId { get; set; }
        public long StatusId { get; set; }
        public decimal Margin { get; set; }
        public decimal EmamiPrice { get; set; }
        public long WorkableQuantity { get; set; }
        public decimal WorkablePrice { get; set; }
        public string Remarks { get; set; }
        public int LoginUserId { get; set; }
       public List<CompetitorAnalysisDetailsAddDto> CompetitorAnalysisDetailsList { get; set; }
    }
}
