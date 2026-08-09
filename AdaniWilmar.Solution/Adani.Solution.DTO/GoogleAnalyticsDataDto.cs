using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class GoogleAnalyticsDataDto : UserIdDto, IAPIInputDTO
    {
        public long TotalLoginsByDistributor { get; set; }
        public long TotalLoginsBySales { get; set; }
        public long TotalEmployeesLoggedIn { get; set; }
        public long TotalNumberOfUsers { get; set; } // RegisteredUserCount
        public long ActiveUserCount { get; set; }//optional
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

}
