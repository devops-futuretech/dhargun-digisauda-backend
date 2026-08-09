using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SAPDataResponseDto
    {
        public bool IsBulkInsert { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public long LoginUserId { get; set; }
        public string SyncFolder { get; set; }
        public string Subject { get; set; }
        public List<string> SourceFileName { get; set; }
        public List<string> LocalFileName { get; set; }
        public List<string> ErrorPdf { get; set; }
        [JsonProperty(PropertyName = "response")]
        public object Response { get; set; }


        public SAPDataResponseDto()
        {
            Response = string.Empty;
            SourceFileName = new List<string>();
            LocalFileName = new List<string>();
            ErrorPdf= new List<string>();
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
