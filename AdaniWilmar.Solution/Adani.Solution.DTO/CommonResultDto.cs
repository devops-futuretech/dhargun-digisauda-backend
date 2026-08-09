using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CommonResultDto
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string FailedRecordName { get; set; }
        public string Message { get; set; }
    }
}
