using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CustomerAccountStatementDto
    {
        public long Totalcount { get; set; }
        public long CustomerUserId { get; set; }
        public bool IsSubmitted { get; set; }
        public long Requestid { get; set; }
        public string CountLimit { get; set; }
    }
}
