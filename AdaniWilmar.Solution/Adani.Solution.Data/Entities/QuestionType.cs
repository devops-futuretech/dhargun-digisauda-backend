using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class QuestionType : Entity
    {
        [Required, MaxLength(250)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public long CreatedBy { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }
    }
}
