using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Territory : Entity
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public int StateId { get; set; }

        public int? SortOrder { get; set; }        

        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [ForeignKey("StateId")]
        public virtual State State { get; set; }
    }
}
