using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class LiftingRequestDeliveryOrderNumberDto
    {
        public long Id { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string SaudaNumber { get; set; }
        public decimal ContractQuantity { get; set; }
        public decimal PendingQuantity { get; set; }
        public decimal LiftingQuantity { get; set; }
        public string ErrorMessage { get; set; }
    }
}
