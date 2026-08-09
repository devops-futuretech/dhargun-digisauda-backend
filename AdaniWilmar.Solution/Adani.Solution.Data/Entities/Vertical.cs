using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Vertical : Auditable
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string SAPCode { get; set; }
    }
}
