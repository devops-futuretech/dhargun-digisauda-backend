using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   
    public class MaterialType 
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ModifiedDate { get; set; }

        public virtual Division Division { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
    }
}
