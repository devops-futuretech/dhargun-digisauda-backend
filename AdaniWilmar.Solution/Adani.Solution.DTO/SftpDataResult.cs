using Adani.Solution.DTO;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SftpDataResult
    {
        [JsonProperty(PropertyName = "response")]
        public object Response { get; set; }
        public List<string> FileName { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }       

        public SftpDataResult()
        {
            Response = string.Empty;
            FileName = new List<string>();
        }
    }
}
