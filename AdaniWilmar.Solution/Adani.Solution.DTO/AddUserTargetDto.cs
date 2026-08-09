using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AddUserTargetDto
    {
        public long AssignedFromId { get; set; }
        public long AssignedToId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public decimal TargetQuanity { get; set; }
        public decimal SchemeQuanity { get; set; }
        public long CreatedBy { get; set; }
    }
}
