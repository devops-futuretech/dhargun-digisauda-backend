using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SchemeDiscountGeographyMapping : Auditable
    {
        public long SchemeDiscountGeographyId { get; set; }
        public long SkuId { get; set; }
        public int CityId { get; set; }
        public long CustomerId { get; set; }
        public long CustomerGroupId { get; set; }
        public bool IsActive { get; set; }

        public virtual City City { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual User Customer { get; set; }
        public virtual CustomerGroups CustomerGroup { get; set; }
        public virtual SchemeDiscountGeography SchemeDiscountGeography { get; set; }
    }
}
