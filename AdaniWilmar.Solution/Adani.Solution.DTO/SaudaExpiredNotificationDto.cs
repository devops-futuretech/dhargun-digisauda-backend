using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SaudaExpiredNotificationDto
    {
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string SaudaNumber { get; set; }
        public string ValidToDate { get; set; }
        public string CreatedDate { get; set; }
    }
}
