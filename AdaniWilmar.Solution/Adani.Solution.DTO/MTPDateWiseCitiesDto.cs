using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MTPDateWiseCitiesDto
    {
        public DateTime Date { get; set; }
        public int TownId { get; set; }
        public string Town { get; set; }

        public List<MTPDateWiseDealersDto> MTPDateWiseDealersDtos { get; set; }
        public string NoVisitHQ { get; set; }
    }
}
