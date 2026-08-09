using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ReasonAddDto
    {
        public string Reason { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public long CreatedBy { get; set; }
    }
}
