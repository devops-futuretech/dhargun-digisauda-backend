using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserIncoTerms : Auditable
    {
        public long UserId { get; set; }

        public long IncoTermsId { get; set; }        
    }
}
