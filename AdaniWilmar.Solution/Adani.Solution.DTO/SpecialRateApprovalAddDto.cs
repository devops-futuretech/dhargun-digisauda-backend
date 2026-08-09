using System.Collections.Generic;
namespace Adani.Solution.DTO
{
    public class SpecialRateApprovalAddDto
    {
        public bool IsLTD { get; set; }
        public IList<SpecialRateAddInputDto> SpecialRateApprovals{ get; set; }
        public SpecialRateApprovalAddDto()
        {
            SpecialRateApprovals = new List<SpecialRateAddInputDto>();
        }
    }
}
