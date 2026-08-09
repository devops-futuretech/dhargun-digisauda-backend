using Adani.Solution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data
{
    public class ChequeInventoryDetail : Auditable
    {
        public string ControllingArea { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string ChequeNo { get; set; }
        public string NameOfBank { get; set; }
        public string BranchName { get; set; }
        public long UserId { get; set; }
    }
}
