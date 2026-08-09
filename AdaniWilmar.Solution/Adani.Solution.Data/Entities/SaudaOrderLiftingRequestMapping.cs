using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adani.Solution.Data.Enum;

namespace Adani.Solution.Data.Entities
{
    public class SaudaOrderLiftingRequestMapping : Auditable
    {
        public long SaudaId { get; set; }
        public long SaudaOrderId { get; set; }
        public string DeliveryOrderNumber { get; set; }

        [DecimalPrecision(18, 4)]
        public decimal LiftingQuantity { get; set; }

        public decimal LiftingQuantityCase { get; set; }
        public int UomId { get; set; }
        public long LiftingRequestDetailId { get; set; }
        public int StatusId { get; set; }

        //[Column(TypeName = "datetime2")]
        //public DateTime LiftingDate { get; set; }
    }
}
