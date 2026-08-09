using Adani.Solution.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SyncDataResult
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public List<string> FilePath { get; set; }
        public SapDataSyncResultDto SapDataSyncResultDtoResult { get; set; }

        public SyncDataResult()
        {
            FilePath = new List<string>();
            SapDataSyncResultDtoResult = new SapDataSyncResultDto();
        }
    }
}
