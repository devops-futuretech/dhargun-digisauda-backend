using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class BdoCompetitorSku : Auditable
    {
        public long BdoCompetitorId { get; set; }
        public string SkuName { get; set; }
        public decimal QuanityPerMt { get; set; }
        public decimal Price { get; set; }

        public virtual BdoCompetitor BdoCompetitor { get; set; }
    
    }
}
