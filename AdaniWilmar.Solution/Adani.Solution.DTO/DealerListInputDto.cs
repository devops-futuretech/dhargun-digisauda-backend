using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DealerListInputDto : VerticalIdDto
    {
        public long LoginUserId { get; set; }

        public long DealerId { get; set; }
        public List<long> BdoIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> NationalHeadIds { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int PageNo { get; set; }
      public string Name { get; set; }
    }
}
