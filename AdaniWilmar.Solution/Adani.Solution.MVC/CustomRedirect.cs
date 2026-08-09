using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Adani.Solution.MVC
{
    public class CustomRedirect : ActionFilterAttribute
    {
        /// <summary>
        /// Code to handle the page redirection, when an user logs out website redirects to login page (user/login).
        /// Initial login, the landing page is robot validation (home/index)
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext context = HttpContext.Current;

            if (context.Session != null)
            {
                if (context.Session.IsNewSession)
                {
                    string sessionCookie = context.Request.Headers["Cookie"];

                    if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        FormsAuthentication.SignOut();
                        if (!string.IsNullOrEmpty(context.Request.RawUrl))
                        {
                            //string redirectTo = string.Format("{0}?ReturnUrl={1}", "~/User/Login", HttpUtility.UrlEncode(context.Request.RawUrl));
                            string redirectTo = "~/User/Login";
                            filterContext.Result = new RedirectResult(redirectTo);
                            return;
                        }
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/User/Login");
                        return;
                    }
                }
            }

            base.OnActionExecuting(filterContext);

        }
    }
}