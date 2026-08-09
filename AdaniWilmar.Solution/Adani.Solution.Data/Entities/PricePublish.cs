using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class PricePublish : Auditable
    {
        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndDate { get; set; }

        public long StatusId { get; set; }

        public string OilTypeId { get; set; }

        public long PlantId { get; set; }

        public bool IsPublish { get; set; }

        public long SaudaBookingTypeId { get; set; }

        public string ErrorMessage { get; set; }

        public virtual SaudaBookingType SaudaBookingType { get; set; }        
        public virtual Depot Plant { get; set; }
    }
}
