using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserSalesSaudaTargetDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string User { get; set; }
        public int FinancialYearId { get; set; }
        public string FinancialYear { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long CreatedBy { get; set; }
        public List<UserSalesSaudaTargetDetailDto> UserSalesSaudaTargetDetail { get; set; }
        public UserSalesSaudaTargetDto()
        {
            UserSalesSaudaTargetDetail = new List<UserSalesSaudaTargetDetailDto>();
        }
    }
}
