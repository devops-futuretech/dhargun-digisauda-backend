using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserTargetDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long AssignedFromId { get; set; }
        public string AssignedFrom { get; set; }
        public long AssignedToId { get; set; }
        public string AssignedTo { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Now;
        public DateTime ToDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long SkuId { get; set; }
        public string Sku { get; set; }
        public decimal TargetQuanity { get; set; }
        public decimal SchemeQuanity { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
