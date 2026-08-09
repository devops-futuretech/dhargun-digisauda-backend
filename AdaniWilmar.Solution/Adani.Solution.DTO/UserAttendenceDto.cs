using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserAttendenceInputDto
    {
        public int LoginUserId { get; set; }
        public int FinancialYear { get; set; }
        public int Month { get; set; }
    }
}
