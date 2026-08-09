using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SupportFilterInputDto : KendoGridResult
    {
        public long LoginUserId { get; set; }
        public int QueryFrom { get; set; }
        public int RaisedBy { get; set; }
        public int StatusId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
