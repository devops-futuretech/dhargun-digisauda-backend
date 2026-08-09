using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AutoAllocationDetailDto : IAPIInputDTO
    {
        public bool IsChecked { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }

        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public decimal ActualDiscount { get; set; }
        public decimal RequestedDiscount { get; set; }
        public long OilTypeId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public int LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public bool IsSentMail { get; set; } = false;
    }
    public class SaveAutoAllocationDetailDto : IAPIInputDTO
    {
        public List<AutoAllocationDetailDto> autoAllocationDetailDtos { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public SaveAutoAllocationDetailDto()
        {
            autoAllocationDetailDtos = new List<AutoAllocationDetailDto>();
        }
    }
    
}
