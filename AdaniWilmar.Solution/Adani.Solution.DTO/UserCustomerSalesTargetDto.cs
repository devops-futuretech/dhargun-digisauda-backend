using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserCustomerSalesTargetDto
    { 
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long? AssignedFromId { get; set; }
        public long? AssignedToId { get; set; }
        public string AssignedToUser { get; set; }

        public long FinancialYearId { get; set; }
        public string FinancialYear { get; set; }

        public long? VerticalId { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganization { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannel { get; set; }
        public string Vertical { get; set; }

        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public string OilTypeCode { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public string ExistRecords { get; set; }

        public long LoginUserId { get; set; }
        public string LoginUserName { get; set; }
        public bool IsCustomerTarget { get; set; }

        public List<UserTargetDetailDto> UserTargetDetail { get; set; }
        public UserCustomerSalesTargetDto()
        {
            UserTargetDetail = new List<UserTargetDetailDto>();
        }
    }
}
