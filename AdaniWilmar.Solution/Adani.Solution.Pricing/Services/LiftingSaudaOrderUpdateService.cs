using Adani.Solution.Console.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Pricing.Services
{
    public class LiftingSaudaOrderUpdateService:BaseService
    {
        public void LiftingSaudaOrderUpdate()
        {
           
            var apiUrl = WebConfig.LiftingUpdateTrigger;
            GetDataAsync(apiUrl);
        }

        public void GamificationDashboard()
        {

            var apiUrl = WebConfig.WebApiUrlGCPApidata;
            GetDataAsync(apiUrl);
        }
    }
}
