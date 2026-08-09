using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Model
{
  public  class FormDto
    {
        public long FormId { get; set; }
        public string FormName { get; set; }
        public bool IsActive { get; set; }
        public bool IsFormStatus { get; set; }
        public long ParentFormId { get; set; }
        public string ParentFormName { get; set; }
    }
}
