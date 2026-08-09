using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class WholesellerBdo : Auditable
    {
        public long DealerId { get; set; }
        public string Name { get; set; }
    }
}
