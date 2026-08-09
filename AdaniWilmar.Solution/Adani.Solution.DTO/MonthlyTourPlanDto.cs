using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthlyTourPlanDto
    {
        public long MTPId { get; set; }
        public string EncryptedId { get; set; }
        public string MTPNumber { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedUser { get; set; }
        public long PJPId { get; set; }
        public long MonthId { get; set; }
        public string ReasonIds { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<MonthlyTourPlanDetailsDto> MonthlyTourPlanDetailList { get; set; }
    }
}
