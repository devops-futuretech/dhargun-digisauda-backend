using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthlyTourPlanAddDto
    {
        public string MTPNumber { get; set; }
        public long StatusId { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public long PJPId { get; set; }
        public long MonthId { get; set; }
        public List<MonthlyTourPlanDetailsDto> MonthlyTourPlanDetails { get; set; }
    }
}
