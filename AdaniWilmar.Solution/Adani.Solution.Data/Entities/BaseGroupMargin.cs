using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class BaseGroupMargin : Auditable
    {
        public BaseGroupMargin()
        {
            this.DerivedGroupMargins = new HashSet<DerivedGroupMargin>();
            this.BaseGroupMarginStates = new HashSet<BaseGroupMarginStates>();
        }

        public long OilTypeId { get; set; }
        public long PackGroupId { get; set; }
        public long CustomerGroupId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }

        public decimal Margin { get; set; }
        public bool IsActive { get; set; }

        public virtual OilType OilType { get; set; }
        public virtual PackGroup PackGroup { get; set; }
        public virtual CustomerGroups CustomerGroup { get; set; }
        public virtual ICollection<DerivedGroupMargin> DerivedGroupMargins { get; set; }
        public virtual ICollection<BaseGroupMarginStates> BaseGroupMarginStates { get; set; }
    }
}
