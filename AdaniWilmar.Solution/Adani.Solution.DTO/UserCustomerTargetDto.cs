using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserCustomerTargetDto
    {
        public long Id { get; set; }
        public long? AssignedFromId { get; set; }
        public long? AssignedToId { get; set; }
        public string AssignedToUser { get; set; }

        public long FinancialYearId { get; set; }
        public string FinancialYear { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public string ExistRecords { get; set; }

        public long LoginUserId { get; set; }
        public string LoginUserName { get; set; }
        public bool IsCustomerTarget { get; set; }

        public List<UserTargetDetailDto> UserTargetDetail { get; set; }
        public UserCustomerTargetDto()
        {
            UserTargetDetail = new List<UserTargetDetailDto>();
        }
    }
}
