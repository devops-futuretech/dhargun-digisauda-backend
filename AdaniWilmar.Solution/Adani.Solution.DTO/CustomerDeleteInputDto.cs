using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CustomerDeleteInputDto : LoginUserIdDto, IAPIInputDTO
    {
        public List<long> CustomerGroupDetailIds { get; set; }
        public long CustomerGroupId { get; set; }
        public List<long> CustomerIds { get; set; }

        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
}
