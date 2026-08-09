using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Line : Auditable
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
