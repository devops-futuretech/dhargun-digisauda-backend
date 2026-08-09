using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SAPAccountStatementDto
    {
        public long Request_Id { get; set; }
        public string SoldToPary { get; set; }
        public string Message { get; set; }
        //public bool PostStatus { get; set; }
        //public string PostMessage { get; set; }
    }
}
