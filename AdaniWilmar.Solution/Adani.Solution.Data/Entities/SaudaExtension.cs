using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaExtension : Auditable
    {
        [Required]
        [ForeignKey("OilType")]
        public long OilTypeId { get; set; }
        [Required]
        [ForeignKey("State")]
        public int StateId { get; set; }
        [Required]
        public long ExtensionDays { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual State State { get; set; }
    }
}
