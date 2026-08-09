using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SAPDepotDto
    {
        public string PlantCode { get; set; }
        public string Name { get; set; }
        public string ADRNR { get; set; }
        public string Street1 { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string CentralArchiving { get; set; }
        public string StateName { get; set; }
        public string Street2 { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string TaxNumber { get; set; }
        public bool IsPlant { get; set; }
    }
}
