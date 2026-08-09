using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class DeleteListCreation : Auditable
    {
        [Required]
        public long DeleteListId { get; set; }
        [Required]
        [MaxLength(4000)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
    }
}
