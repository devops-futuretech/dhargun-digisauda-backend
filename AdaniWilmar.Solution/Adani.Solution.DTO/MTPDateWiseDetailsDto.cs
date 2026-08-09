using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MTPDateWiseDetailsDto
    {
        public long Id { get; set; }
        public long MTPId { get; set; }
        public string Date { get; set; }
        public int TownId { get; set; }
        public string Town { get; set; }
        public string Area { get; set; }
        public string DealerId { get; set; }
        public string Dealer { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
    }
}
