using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class BaseGroupMarginStates : Auditable
    {
        public long BaseGroupMarginId { get; set; }

        public int StateId { get; set; }

        public bool IsActive { get; set; }
    }
}
