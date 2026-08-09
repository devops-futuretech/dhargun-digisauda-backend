using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestStatusChangeDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<long> LiftingIds { get; set; }
        public List<string> EncryptedIds { get; set; }

        public LiftingRequestStatusChangeDto()
        {
            LiftingIds = new List<long>();
            EncryptedIds = new List<string>();
        }

    }
}
