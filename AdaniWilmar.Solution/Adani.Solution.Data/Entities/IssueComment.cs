using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class IssueComment : Auditable
    {
        [Required]
        public long SupportId { get; set; }
        public string Comments{ get; set; }
    }
}
