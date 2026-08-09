using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DepotCostReportDto
    {
        public DateTime DateOfUpload { get; set; }
        public string DepotName { get; set; }
        public string DepotCode { get; set; }
        public int LoginUserId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string StateName { get; set; }
        public decimal DepotCostPerCase { get; set; }
        public decimal DepotCostPerMT { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsPublished { get; set; }
    }
}
