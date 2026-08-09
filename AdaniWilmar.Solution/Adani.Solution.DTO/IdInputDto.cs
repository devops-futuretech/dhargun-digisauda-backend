using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class IdInputDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long PackGroupId { get; set; }
        //public bool IsBulkPack { get; set; }

        public long baseCustomerGroupId { get; set; }

        public List<long> IdList { get; set; }
    }
}
