using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MTPDateWiseDealersDto
    {
        public DateTime Date { get; set; }
        public int TownId { get; set; }
        public string DealerId { get; set; }
        public string Dealer { get; set; }
    }

    public class MTPDateWiseNoVisitDto
    {
        public DateTime Date { get; set; }
        public int TownId { get; set; }
        public int? NoVisitHQId { get; set; }
        public string NoVisitHQ { get; set; }
    }
}
