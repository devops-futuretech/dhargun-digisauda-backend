using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CompetitorAnalysisViewDto
    {
        public long Id { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long? OilTypeId { get; set; }
        public string OilType { get; set; }
        public string OilTypeCode { get; set; }
        public long? StatusId { get; set; }
        public string Status { get; set; }
        public decimal Margin { get; set; }
        public decimal EmamiPrice { get; set; }
        public long WorkableQuantity { get; set; }
        public decimal WorkablePrice { get; set; }
        public string Remarks { get; set; }

        public bool HasAccessToProceed { get; set; }
        public int ApprovalsCount { get; set; }

        public string RequestedBy { get; set; }
        public string RequestedTo { get; set; }

        public decimal CushionMargin { get; set; }
        public decimal ProfitMargin { get; set; }
        public decimal TotalCushionProfitMargin { get; set; }
        public decimal CalculatedFinalMargin { get; set; }


        public List<CompetitorAnalysisDetailsViewDto> CompetitorAnalysisDetailsDtoList { get; set; }

        public int RoleId { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
}
