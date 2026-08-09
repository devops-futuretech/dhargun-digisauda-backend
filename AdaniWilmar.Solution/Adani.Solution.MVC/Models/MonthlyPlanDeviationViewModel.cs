using Adani.Solution.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.Solution.MVC.Models
{
    public class MonthlyPlanDeviationViewModel
    {
        public List<MonthlyPlanDeviationListDto> MonthlyPlanDeviationListDto { get; set; }
        public long CreatedBy { get; set; }
        public bool IsApprovar { get; set; }
        public long ApprovedBy { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}