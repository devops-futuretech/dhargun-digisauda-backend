using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class CustomerGroupUploadDto : CommonResultDto
    {
        public string CustomerGroupName { get; set; }
        public string VerticalCode { get; set; }
        public string IsActive { get; set; }
        public string IsBaseGroup { get; set; }
        public string CustomerCode { get; set; }
        public long CreatedBy { get; set; }

    }
}
