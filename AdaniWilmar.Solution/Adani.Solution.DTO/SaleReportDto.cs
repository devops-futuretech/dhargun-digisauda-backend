using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaleReportDto
    {
        public long PlantId { get; set; }
        public string Name { get; set; }
        public long MaterialId { get; set; }
        public string MaterialDescription { get; set; }
        public string Message { get; set; }
    }

    public class SalesExportDto
    {
        public string PlantName { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public string MaterialDescription { get; set; }
        public string SLoc { get; set; }
        public string BUn { get; set; }
        public string Unrestricted { get; set; }
        public string QualityInsp { get; set; }
        public string Blocked { get; set; }
        public string TransTfr { get; set; }
        public string Message { get; set; }
    }
}
