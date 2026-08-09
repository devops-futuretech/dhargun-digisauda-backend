using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FormRemarksDto 
    {
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class FormRemarkInputDto
    {
        public long FormId { get; set; }
        public int RemarkTypeId { get; set; }
    }
}
