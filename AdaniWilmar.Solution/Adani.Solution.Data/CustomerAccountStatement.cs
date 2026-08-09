using Adani.Solution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data
{
    public class CustomerAccountStatement : Auditable
    {
        public long Id { get; set; }
        public long CustomerUserId { get; set; }
        public bool IsSubmitted { get; set; }
    }
}
