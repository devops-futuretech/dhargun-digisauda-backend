using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.Solution.MVC.Models
{
    public class PJPMonthValueMapping
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public int NoOfVisit { get; set; }
        public int RetailerId { get; set; }
    }
}