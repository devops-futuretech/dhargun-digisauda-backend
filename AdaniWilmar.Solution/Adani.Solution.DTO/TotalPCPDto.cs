using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TotalPCPDto
    {
        public long CityId { get; set; }
        public string City { get; set; }
        public string Dealers { get; set; }
        public long NoOfDealers { get; set; }
        public decimal NoOfVisit { get; set; }
        public decimal HQVisitCount { get; set; }
        public long BDOId { get; set; }
        public string BDOName { get; set; }
    }
}
