using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Adani.Solution.API.Infrastructure
{
    public class SAPConstants
    {
        public static string Username = ConfigurationManager.AppSettings["Username"];
        public static string Password = ConfigurationManager.AppSettings["Password"];
        public static bool IsTokenValidationAllowed => Convert.ToBoolean(ConfigurationManager.AppSettings["IsTokenValidationAllowed"]);
    }
}