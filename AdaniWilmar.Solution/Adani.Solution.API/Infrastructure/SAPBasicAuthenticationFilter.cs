using GMCore.Helper;
using System.Web.Http.Controllers;

namespace Adani.Solution.API.Infrastructure
{
    public class SAPBasicAuthenticationFilter : BasicAuthenticationFilter
    {
        public SAPBasicAuthenticationFilter()
        {
        }

        public SAPBasicAuthenticationFilter(bool active) : base(active)
        {
        }

        /// <summary>
        /// Validate Middleware SAP Credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {

            var passwordEry = UtilityHelper.ConvertMd5ToString(password, SecurityConstants.EncryptionKey);
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(passwordEry) && SAPConstants.Username.Equals(username) && SAPConstants.Password.Equals(passwordEry))
            {
                return true;
            }
            return false;
        }
    }
}