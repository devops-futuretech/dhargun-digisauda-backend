using Adani.Solution.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class QpsDiscount : Auditable
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public long SalesOrgId { get; set; }

        public long DistributionChannelId { get; set; }

        public long DivisionId { get; set; }

        public bool IsActive { get; set; }
    }
}
