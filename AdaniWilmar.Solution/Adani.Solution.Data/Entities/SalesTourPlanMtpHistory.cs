using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SalesTourPlanMtpHistory : Auditable
    {
        public long DealerId { get; set; }
        public int CityId { get; set; }
        public string Area { get; set; }
        public long HeadquartersId { get; set; }
        public string Remarks { get; set; }
        public long MonthlyTourPlanDetailId { get; set; }

        public int InHQNoVisit { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime TourDate { get; set; }

        public bool IsDataChanged { get; set; }

        //public virtual City City { get; set; }
        //public virtual User Dealer { get; set; }
        //public virtual Headquarters Headquarters { get; set; }
    }
}
