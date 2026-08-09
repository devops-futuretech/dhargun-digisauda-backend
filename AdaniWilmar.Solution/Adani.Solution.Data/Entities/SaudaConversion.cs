using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SaudaConversion : Auditable
    {
        public long SaudaOrderId { get; set; }
        public long DealerId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ExtendToDate { get; set; }
        public long? StatusId { get; set; }
        public long? ExtensionStatusId { get; set; }
        public bool IsConversion { get; set; }
        public bool IsExtension { get; set; }

        public virtual Status Status { get; set; }
        public virtual Status ExtensionStatus { get; set; }
        //public virtual Sauda Sauda { get; set; }
        public virtual SaudaOrder SaudaOrder { get; set; }
        public virtual User Dealer { get; set; }
    }
}
