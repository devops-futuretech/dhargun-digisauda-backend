using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserLoginHistory : Auditable
    {

        public long LoginUserId { get; set; }

        public DateTime LoginDate { get; set; }

    }
}
