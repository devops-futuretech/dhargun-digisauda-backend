using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestMobileOutputDto
    {
        public int ListCount { get; set; }
        public List<LiftingRequestMobileListDto> LiftingRequestList { get; set; }
    }

    public class LiftingRequestDetailMobileOutputDto
    {
        public int ListCount { get; set; }
        public List<LiftingRequestDetailMobileListDto> LiftingRequestDetailList { get; set; }
    }
}
