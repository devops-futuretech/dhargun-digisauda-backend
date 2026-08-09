using Adani.Solution.MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adani.Solution.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class UserTokenAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var context = new Settings();
            var userProfile = context.GetUserProfile(httpContext.Request);
            if (userProfile == null)
            {
                return true;
            }
            httpContext.User = Settings.GetIdentity(userProfile);
            httpContext.Request.Headers.Add("Authorization", userProfile.LoginToken);

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        //redirectTo = Settings.Slash + Settings.AccountController + Settings.Slash + Settings.RedirectToLocalAction
                        redirectTo = "/"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}