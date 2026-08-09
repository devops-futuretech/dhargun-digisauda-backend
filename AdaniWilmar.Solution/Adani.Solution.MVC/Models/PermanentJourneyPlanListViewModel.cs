using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.Solution.MVC.Models
{
    public class PermanentJourneyPlanListViewModel
    {
        public int RetailerId { get; set; }
        public string Retailername { get; set; }
        public List<PJPMonthValueMapping> PJPMonthValueMapping { get; set; }
        public List<PJPMonthValueUpdateViewModel> PJPMonthValueUpdate { get; set; }
        public PermanentJourneyPlanListViewModel()
        {
            PJPMonthValueUpdate = new List<PJPMonthValueUpdateViewModel>();
        }
    }
}