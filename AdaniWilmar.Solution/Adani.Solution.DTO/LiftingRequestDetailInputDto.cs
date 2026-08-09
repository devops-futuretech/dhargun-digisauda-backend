using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LiftingRequestDetailInputDto
    {
        public long LiftingRequestId { get; set; }
        public long SKUId { get; set; }
        //public long OilTypeId { get; set; }
        public long SaudaOrderId { get; set; }
        public decimal LiftingQuantity { get; set; }
        public long LoginUserId { get; set; }
        public decimal MaxAllowable { get; set; }
        public string SaudaNumber { get; set; }
    }
}
