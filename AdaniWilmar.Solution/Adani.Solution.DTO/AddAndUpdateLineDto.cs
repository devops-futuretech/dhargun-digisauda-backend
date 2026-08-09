using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AddAndUpdateLineDto : UserIdDto,IAPIInputDTO
    {
        public long LineId { get; set; }
        public string EncryptedId { get; set; }
        public string LineName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
}
