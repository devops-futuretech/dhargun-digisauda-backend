using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ExcelReportFilterDto : RoleIdDto
    {
        
        public long StatusId { get; set; }
        public string StateIds { get; set; }
        public long MarginTypeId { get; set; }
        public long VerticalIds { get; set; }
        public string BDOIds { get; set; }
        public string StatusIds { get; set; }
        public long PlantId { get; set; }
    }
}
