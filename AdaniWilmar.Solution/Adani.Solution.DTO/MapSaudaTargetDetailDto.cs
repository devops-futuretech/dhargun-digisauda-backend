using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class MapSaudaTargetDetailDto
    {
        public long? AssignedFromId { get; set; }
        public string AssignedFromUser { get; set; }

        public long? AssignedToId { get; set; }
        public string AssignedToUser { get; set; }

        public long FinancialYearId { get; set; }
        public string FinancialYear { get; set; }

        public long? VerticalId { get; set; }
        public long? SalesOrganizationId { get; set; }
        public long? DistributionChannelId { get; set; }
        public string Vertical { get; set; }

        public long OilTypeId { get; set; }
        public string OilType { get; set; }

        public long MonthId { get; set; }
        public string Month { get; set; }
        public string MonthAndYear { get; set; }
        public long Year { get; set; }

        public decimal Target { get; set; }

        public long LoginUserId { get; set; }
    }
}
