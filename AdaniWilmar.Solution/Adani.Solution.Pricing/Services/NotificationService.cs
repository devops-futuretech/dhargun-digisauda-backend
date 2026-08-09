using Adani.Solution.Console.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Pricing.Services
{
    public class NotificationService : BaseService
    {
        public void SendNotificationAsync()
        {          
           
            var apiUrl = WebConfig.WebApiUrlNotification;
            GetDataAsync( apiUrl);
        }

        public void PendingContractAsync()
        {
            var apiUrl = WebConfig.PendingContractAutoTrigger;
            PostAsyncWithBaicAuthentication(apiUrl,null);
        }

        public void CustomerLedgerRequest()
        {
            var apiUrl = WebConfig.CustomerLedgerRequestAutoTrigger;
            PostAsyncWithBaicAuthentication(apiUrl, null);
        }

        public void EmployeeRequestActiveUsers()
        {
            var apiUrl = WebConfig.EmployeeRequestActiveUsersAutoTrigger;
            PostAsyncWithBaicAuthentication(apiUrl, null);
        }

        public void EmployeeRequestInActiveUsers()
        {
            var apiUrl = WebConfig.EmployeeRequestInActiveUsersAutoTrigger;
            PostAsyncWithBaicAuthentication(apiUrl, null);
        }

        public void SaudaExpiredNotification()
        {
            var apiUrl = WebConfig.SaudaExpiredNotificationAutoTrigger;
            PostAsyncWithBaicAuthentication(apiUrl, null);
        }

        public void OverDueNotification()
        {
            var apiUrl = WebConfig.OverDueNotificationAutoTrigger;
            PostAsyncWithBaicAuthentication(apiUrl, null);
        }
    }
}
