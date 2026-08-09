using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Bulletin : Auditable
    {
        public Bulletin()
        {
            this.BulletinMedia = new HashSet<BulletinMedia>();
        }
        [MaxLength(1000)]
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public long? ReviewedBy { get; set; }
        public bool? IsApproved { get; set; }
        [Required]
        public int ContentTypeId { get; set; }

        public virtual ContentType ContentType { get; set; }
        public virtual ICollection<BulletinMedia> BulletinMedia { get; set; }
    }
}
