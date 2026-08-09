using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ReasonDto
    {
        public long Id { get; set; }
        public string Reason { get; set; }
        public long ReasonId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
    public class MTPDealerDto
    {
        public long Id { get; set; }

        public List<MTPDealerDetailDto> MTPDealerDetail { get; set; }
        public MTPDealerDto()
        {
            MTPDealerDetail = new List<MTPDealerDetailDto>();
        }
    }

    public class MTPDealerDetailDto
    {
        public string Dealer { get; set; }
        public long Id { get; set; }
    }
}
