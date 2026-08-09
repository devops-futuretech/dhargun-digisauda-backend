using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class CustomerGroupFive: Auditable
    {
        [Required]
        public string GroupCode { get; set; }
        [Required]
        public string GroupName { get; set; }
        public bool IsActive { get; set; }
    }
}
