using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DealersLiftingRequestInputDto: KendoGridResult
    {
        public long DealerId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long StatusId { get; set; }
        public bool IsToReturnInactiveData { get; set; }
        public long StateId { get; set; }
        public bool IsFilter { get; set; }
        public List<long> StateIds { get; set; }
    }

    public class LiftingRequestListsInputDto : LoginUserIdDto
    {
        public List<long> NationalHeadIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> BdoIds { get; set; }
        public List<long> DealerIds { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long StatusId { get; set; }
        public long StateId { get; set; }
        public List<long> StateIds { get; set; }
    }
}
