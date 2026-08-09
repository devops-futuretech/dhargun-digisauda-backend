using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SaudaExtensionNewAddDto
    {
        public List<SaudaExtensionDto> SaudaExtensionList { get; set; }
        public long LoginUserId { get; set; }
        public string Remarks { get; set; }
    }
}
