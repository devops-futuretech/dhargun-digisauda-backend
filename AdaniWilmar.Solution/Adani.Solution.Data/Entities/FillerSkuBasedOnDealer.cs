using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
   public class FillerSkuBasedOnDealer : Auditable
    {
        public long SkuId { get; set; }
        public long PackTypeId { get; set; }
        public long UserId { get; set; }
        public decimal BidQuantityInCases { get; set; }
        public virtual PackType PackType { get; set; }
        public virtual Sku Sku { get; set; }
        public virtual User User { get; set; }
    }
}
