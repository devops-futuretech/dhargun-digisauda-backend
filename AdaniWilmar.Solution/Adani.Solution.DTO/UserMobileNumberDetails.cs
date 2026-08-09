using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserMobileNumberDetails
    {
        public long DailerId { get; set; }
        public string DailerName { get; set; }
        public string DailerCode { get; set; }
        public string DailerMobileNumber { get; set; }
        public List<ReceiverMobileNumberDetails> ReceiverMobileNumberDetailsList { get; set; }
        public UserMobileNumberDetails()
        {
            ReceiverMobileNumberDetailsList = new List<ReceiverMobileNumberDetails>();
        }
    }

    public class ReceiverMobileNumberDetails
    {
        public long ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverCode { get; set; }
        public string ReceiverMobileNumber { get; set; }
        public long DialerId { get; set; }
        public string DialerMobileNumber { get; set; }
    }
}
