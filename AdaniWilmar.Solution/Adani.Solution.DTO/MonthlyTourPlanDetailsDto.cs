using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MonthlyTourPlanDetailsDto
    {
        public long Id { get; set; }
        public long MTPId { get; set; }
        public string EncryptedId { get; set; }
        public string Date { get; set; }
        public DateTime MTPDate { get; set; }
        public long DayId { get; set; }
        public string Day { get; set; }
        public int TownId { get; set; }
        public string Town { get; set; }
        public string Area { get; set; }
        public string DealerId { get; set; }
        public string Dealer { get; set; }
        public long HeadquartersId { get; set; }
        public string Headquarters { get; set; }
        public string Remarks { get; set; }
        public string VisitRemarks { get; set; }
        public long CreatedBy { get; set; }
        public long IsDeleted { get; set; }
        public string TravelTo { get; set; }
        public int InHQNoVisitId { get; set; }
        public string InHQNoVisitName { get; set; }
    }
}
