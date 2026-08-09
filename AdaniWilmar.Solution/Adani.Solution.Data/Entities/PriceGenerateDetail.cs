using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class PriceGenerateDetail : Auditable
    {
        public long PriceGenerateId { get; set; }
        public string OilTypeId { get; set; }
        public string PackGroupId { get; set; }
        public long PlantId { get; set; }
        public string ZoneId { get; set; }
        public int StateId { get; set; }
        public int StatusId { get; set; }
        public int TaskStatusId { get; set; }
        public bool IsPublish { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndDate { get; set; }

        public string ErrorMessage { get; set; }
        public int ErrorMessageCount { get; set; }

        public decimal CounterBidLimit { get; set; }
        public decimal BpCpJump { get; set; }
        public decimal XMargin { get; set; }
        public long BiddingWindowId { get; set; }
        public long CustomerGroupId { get; set; }

        public virtual Depot Plant { get; set; }       
        
    }
}
