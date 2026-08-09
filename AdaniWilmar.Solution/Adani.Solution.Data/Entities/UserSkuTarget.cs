using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserSkuTarget : Auditable
    {
        public long AssignedFromId { get; set; }
        public long AssignedToId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public int Quarter { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TargetQuanity { get; set; }
       
    }
}
