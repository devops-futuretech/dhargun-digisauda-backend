using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SAPfileMove
    {
        public string DestinationPath { get; set; }
        public string SourcePath { get; set; }
        public string FileName { get; set; }
        public bool IsSuccess { get; set; }
    }
}
