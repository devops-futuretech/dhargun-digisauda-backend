using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class CompetitorAnalysisDto
    {
        public long Id { get; set; }
        public long? VerticalId { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public decimal Margin { get; set; }
        public decimal EmamiPrice { get; set; }
        public long WorkableQuantity { get; set; }
        public decimal WorkablePrice { get; set; }
        public string Remarks { get; set; }
        public long CompetitorId { get; set; }

        List<CompetitorAnalysisDetailsDto> CompetitorAnalysisDetailsDtos { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
}
