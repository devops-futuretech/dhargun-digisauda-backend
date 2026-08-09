using System.Collections.Generic;
namespace Adani.Solution.DTO
{
    public class CompetitorAnalysisInputDto
    {
        public IList<CompetitorAnalysisAddDto> CompetitorAnalysisList { get; set; }
        public CompetitorAnalysisInputDto()
        {
            CompetitorAnalysisList = new List<CompetitorAnalysisAddDto>();
        }
    }
}
