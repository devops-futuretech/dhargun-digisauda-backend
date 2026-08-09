using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class UserOilTypeTargetDto
    {
        public long Id { get; set; }

        public long? AssignedFromUserId { get; set; }
        public long AssignedToUserId { get; set; }
        public string AssignedToUser { get; set; }

        public long FinancialYearId { get; set; }
        public string FinancialYear { get; set; }

        public long? VerticalId { get; set; }
        public string Vertical { get; set; }

        public long OilTypeId { get; set; }
        public string OilType { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long LoginUserId { get; set; }

        public List<UserTargetDetailDto> UserOiltypeTargetDetail { get; set; }

        public UserOilTypeTargetDto()
        {
            UserOiltypeTargetDetail = new List<UserTargetDetailDto>();
        }
    }
}
