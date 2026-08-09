using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SupportCategoryDto
    {
        public IList<DropDownDto> IssueTypes { get; set; }
        public IList<DropDownDto> SeverityTypes { get; set; }
        public IList<DropDownDto> Modules { get; set; }

        public SupportCategoryDto()
        {
            IssueTypes = new List<DropDownDto>();
            SeverityTypes = new List<DropDownDto>();
            Modules = new List<DropDownDto>();
        }
    }
}
