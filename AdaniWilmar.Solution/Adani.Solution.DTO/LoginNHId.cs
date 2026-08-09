using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LoginNHId : LoginUserIdDto
    {
        public long ZHId { get; set; }
        public List<long> ZHIds { get; set; }
        public List<long> BDOIds { get; set; }
    }
}
