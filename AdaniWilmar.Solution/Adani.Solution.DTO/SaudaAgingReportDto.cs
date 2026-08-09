using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaAgingReportDto
    {
        public int SaudaAging { get; set; }
        public int DepotId { get; set; }
        public string Party { get; set; }
        public string PartyName { get; set; }
        public int CityId { get; set; }
        public string MaterialDescription { get; set; }


    }

    public class SaudaAgingReportExportDto
    {
        public int SaudaAging { get; set; }
    
        public string Party { get; set; }
        public string PartyName { get; set; }
        public string City { get; set; }
        public string MaterialDescription { get; set; }
        public string BaseDepot { get; set; }
        public DateTime PODate { get; set; }
        public DateTime Date { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ContractEndDate { get; set; }
        public int ContractQuantity { get; set; }
        public int OSQuantity { get; set; }

    }

}
