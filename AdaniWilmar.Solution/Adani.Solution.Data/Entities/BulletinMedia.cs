using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class BulletinMedia : Auditable
    {
        [Required, MaxLength(1500)]
        public string MediaPath { get; set; }
        [Required]
        public int MediaTypeId { get; set; }
        [Required]
        public long BulletinId { get; set; }

        public virtual MediaType MediaType { get; set; }
        public virtual Bulletin Bulletin { get; set; }
    }
}
