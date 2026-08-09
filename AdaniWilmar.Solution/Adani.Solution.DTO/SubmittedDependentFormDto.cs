using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SubmittedDependentFormDto
    {
        public long SubmittedFormId { get; set; }
        public long FormId { get; set; }
        public string FormName { get; set; }
        public string DemonstratedBy { get; set; }
        public long DemoId { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<SectionDto> Sections { get; set; }
        public SubmittedDependentFormDto()
        {
            Sections = new List<SectionDto>();
        }
    }
}
