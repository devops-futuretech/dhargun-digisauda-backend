using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class IncoTermsDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public bool IsActive { get; set; }
    }
}
