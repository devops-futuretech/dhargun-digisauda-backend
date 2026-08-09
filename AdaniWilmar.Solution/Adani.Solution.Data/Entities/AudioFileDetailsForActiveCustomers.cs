using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class AudioFileDetailsForActiveCustomers : Auditable
    {
        public long UserId { get; set; }
        public int MediaTypeId { get; set; }
        public string AudioFileName { get; set; }
        public string ImagePaths { get; set; }
        public virtual MediaType MediaType { get; set; }
        public string DialerMobileNumber { get; set; }
        public string ReceiverMobileNumber { get; set; }
        public long DialerId { get; set; }
        public long ReceiverId { get; set; }
        public string CallRecordedFileName { get; set; }
        public int CallDuation { get; set; }
        public string CallStartTime { get; set; }
    }
}
