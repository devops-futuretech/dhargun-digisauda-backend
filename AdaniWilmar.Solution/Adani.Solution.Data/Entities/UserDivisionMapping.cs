using Adani.Solution.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserDivisionMapping:Auditable
    {
        public long UserId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        [DecimalPrecision(18, 4)]
        public decimal? SaudaLimit { get; set; }
        public int? SaudaValidityPeriod { get; set; }
        public virtual User User { get; set; }
        public virtual Division Division { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual SalesOrganization SalesOrganization { get; set; }
        
    }
}
