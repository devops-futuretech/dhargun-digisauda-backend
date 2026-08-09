using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AddPendingSaudaRemarksDto
    {
        public long SaudaId { get; set; }
        public long DealerId { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
    }
}
