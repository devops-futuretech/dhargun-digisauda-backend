using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PJPApprovalInformationDto
    {
        public long PJPId { get; set; }
        public long StatusId { get; set; }
        public long UserId { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
    }
}
