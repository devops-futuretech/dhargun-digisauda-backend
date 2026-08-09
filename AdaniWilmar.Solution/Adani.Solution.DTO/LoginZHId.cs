using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LoginZHId:LoginUserIdDto
    {
        public long BDOId { get; set; }
        public List<long> BDOIds { get; set; }
        public long ZHId { get; set; }
    }
}
