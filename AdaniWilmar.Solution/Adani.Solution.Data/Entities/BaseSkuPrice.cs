using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class BaseSkuPrice : Auditable
    {
        public long PriceGenerateDetailId { get; set; }

        public long CustomerGroupId { get; set; }

        public long SkuId { get; set; }

        public int BaseSkuTaskStatusId { get; set; }

        public int DerivedSkuTaskStatusId { get; set; }

        public long ParentId { get; set; }

        public Guid GuId { get; set; }
    }
}
