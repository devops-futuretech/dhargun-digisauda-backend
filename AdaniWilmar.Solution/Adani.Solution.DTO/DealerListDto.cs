using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DealerListOutputDto
    {
        public int ListCount { get; set; }

        public List<DealerListDto> DealerList { get; set; }

    }

    public class DealerListDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string MobileNumber2 { get; set; }
        public string Email { get; set; }
        public decimal SaudaLimit { get; set; }
    }
}
