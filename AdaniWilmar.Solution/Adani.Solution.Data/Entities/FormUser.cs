using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class FormUser : EntityLong
    {
        [Required]
        public long FormId { get; set; }
        [Required]
        public long UserId { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
        public virtual Form Form { get; set; }
    }
}
