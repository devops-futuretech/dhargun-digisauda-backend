using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class GPJumpDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long? VerticalId { get; set; }
        public string Vertical { get; set; }
        public long? OilTypeId { get; set; }
        public string OilType { get; set; }
        public List<long> OilTypeIds { get; set; }
        public long OilPackingTypeId { get; set; }
        public List<long> OilPackingTypeIds { get; set; }
        public string OilPackingType { get; set; }
        public int StartValue { get; set; }
        public int EndValue { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
