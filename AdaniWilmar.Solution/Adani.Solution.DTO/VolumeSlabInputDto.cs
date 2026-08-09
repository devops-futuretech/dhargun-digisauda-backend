using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class VolumeSlabInputDto : LoginUserIdDto, IAPIInputDTO
    {
        public long VolumeDiscountSlabId { get; set; }

        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
}
