using System;
using System.Globalization;
using System.Resources;
using System.Web;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.Security.Application;
using Adani.Solution.DTO.Enums;
using GMCore.Helper;
using Adani.Solution.DTO;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Adani.Solution.MVC.Helpers
{
    public class Helper
    {
        static ResourceManager res_man = new ResourceManager("Adani.Solution.MVC.Resource.Resources", typeof(Helper).Assembly);

        public static string GetResourceString(string input)
        {
            try
            {
                var cul = CultureInfo.CreateSpecificCulture(GetCurrentCulture());
                var inputValue = res_man.GetString(input, cul);
                return !string.IsNullOrEmpty(inputValue) ? inputValue : string.Empty;
            }
            catch (Exception)
            {
                // ignored
            }

            return string.Empty;
        } 

        public static string GetMsgRequired(string input)
        {
            var cul = CultureInfo.CreateSpecificCulture(GetCurrentCulture());
            try
            {
                var msgRequired = "msg_CommonRequired";
                var msgRequiredValue = res_man.GetString(msgRequired, cul);
                var inputValue = res_man.GetString(input, cul);
                if (!string.IsNullOrEmpty(msgRequiredValue) && !string.IsNullOrEmpty(inputValue))
                    return string.Format(msgRequiredValue, inputValue.ToLower());
            }
            catch (Exception)
            {
                // ignored
            }
            return string.Empty;
        }

        public static string GetSelectMsgRequired(string input)
        {
            var cul = CultureInfo.CreateSpecificCulture(GetCurrentCulture());
            try
            {
                var msgRequired = "msg_CommonSelectRequired";
                var msgRequiredValue = res_man.GetString(msgRequired, cul);
                var inputValue = res_man.GetString(input, cul);
                if (!string.IsNullOrEmpty(msgRequiredValue) && !string.IsNullOrEmpty(inputValue))
                    return string.Format(msgRequiredValue, inputValue.ToLower());
            }
            catch (Exception)
            {
                // ignored
            }
            return string.Empty;
        }

        public static string GetPlaceHolderSelectMsg(string input)
        {
            var cul = CultureInfo.CreateSpecificCulture(GetCurrentCulture());
            try
            {
                var msgRequired = "msg_PlhoCommonSelect";
                var msgRequiredValue = res_man.GetString(msgRequired, cul);
                var inputValue = res_man.GetString(input, cul);
                if (!string.IsNullOrEmpty(msgRequiredValue) && !string.IsNullOrEmpty(inputValue))
                    return string.Format(msgRequiredValue, inputValue.ToLower());
            }
            catch (Exception)
            {
                // ignored
            }
            return string.Empty;
        }

        /// <summary>
        /// Method to get resource value
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorMessageCode"></param>
        /// <returns></returns>
        public static string GetResourceFor(string code, string errorMessageCode = "")
        {
            string messageString;
            if (code == "tngsqE000" || (string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(errorMessageCode)))
            {
                messageString = GetResourceString(errorMessageCode);
            }
            else
            {
                messageString = GetResourceString(code);
            }
            return messageString;
        }

        public static string GetCurrentCulture()
        {
            var cultureInfo = "en";
            if (HttpContext.Current.Session["CultureInfo"] != null)
                cultureInfo = Convert.ToString(HttpContext.Current.Session["CultureInfo"]);

            return cultureInfo;
        }

        /// <summary>
        /// Method to verify the user cliams
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        public static bool CheckClaims(Claims claim)
        {
            var isValid = false;
            if (HttpContext.Current.Session["UserClaims"] != null)
            {
                var userClaims = (List<UserClaimsDto>)HttpContext.Current.Session["UserClaims"];
                if (userClaims != null)
                {
                    if (userClaims.Any(_ => _.Name == UtilityHelper.GetEnumDescription(Claims.ManageOrganization) && _.IsApplied))
                        return true;

                    isValid = userClaims.Any(_ => _.Name == UtilityHelper.GetEnumDescription(claim) && _.IsApplied);
                }
            }
            return isValid;
        }

        /// <summary>
        /// Sanitize a model part of penetration fixes (sh)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T SanitizeModel<T>(T model)
        {
            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string) && !property.Name.ToLower().Contains("encrypted") && !property.Name.ToLower().Contains("qrcode") && !property.Name.ToLower().Contains("productidswithquantity"))
                {
                    var value = Sanitizer.GetSafeHtmlFragment(Convert.ToString(property.GetValue(model, null)));
                    property.SetValue(model, Convert.ChangeType(value, property.PropertyType), null);
                }
            }
            return model;
        }

        /// <summary>
        /// Method Get Zero Validation Error
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetZeroValidationError(string input)
        {
            var cul = CultureInfo.CreateSpecificCulture(GetCurrentCulture());
            try
            {
                var msgRequired = "msg_ZeroValidationError";
                var msgRequiredValue = res_man.GetString(msgRequired, cul);
                var inputValue = res_man.GetString(input, cul);
                if (!string.IsNullOrEmpty(msgRequiredValue) && !string.IsNullOrEmpty(inputValue))
                    return string.Format(msgRequiredValue, inputValue.ToLower());
            }
            catch (Exception)
            {
                // ignored
            }
            return string.Empty;
        }

        public static string DateTimeFormat(DateTime date)
        {
            var formatedDate = "";
            if (date != null)
            {
                formatedDate = date.ToString("dd-MMM-yyyy");
            }
            return formatedDate;
        }

        public static string DateTimeFormatWithMinutes(DateTime date)
        {
            var formatedDate = "";
            if (date != null)
            {
                formatedDate = date.ToString("dd-MMM-yyyy hh:mm tt");
            }
            return formatedDate;
        }

        public static string NullableDateTimeFormat(DateTime? date)
        {
            var formatedDate = "";
            if (date != null)
            {
                DateTime date1 = (DateTime)date;
                formatedDate = date1.ToString("dd-MMM-yyyy");
            }
            return formatedDate;
        }

        public static decimal DecimalFormat4(decimal? value)
        {
            if (value.HasValue)
            {
                return Convert.ToDecimal(string.Format("{0:0.0000}", value));
            }
            return 0;
        }

        /// <summary>
        /// Method to convert enum to list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> EnumToList<T>()
        {
            var enumType = typeof(T);
            var enumValArray = Enum.GetValues(enumType);
            var enumValList = new List<T>(enumValArray.Length);
            foreach (int value in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, Convert.ToString(value)));
            }
            return enumValList;
        }

        /// <summary>
        /// Method to get enum description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }


    /// <summary>
    /// Method to check roles (sh)
    /// </summary>


    //public class AuthorizeRolesAttribute : AuthorizeAttribute
    //{
    //    private Role[] roleValues { set; get; }
    //    public AuthorizeRolesAttribute(params Role[] roleValues)
    //    {
    //        this.roleValues = roleValues;
    //    }

    //    protected override bool AuthorizeCore(HttpContextBase httpContext)
    //    {
    //        bool isInRole = false;

    //        foreach (var roleValue in roleValues)
    //        {
    //            //var role = Enum.GetName(typeof(Role), roleValue);
    //            var role = EnumHelper.GetEnumDescription(roleValue);
    //            isInRole = httpContext.User.IsInRole(role);
    //            if (isInRole) break;
    //        }
    //        return isInRole;
    //    }
    //}

    public class AuthorizeClaimsAttribute : AuthorizeAttribute
    {
        private Claims[] claimValues { set; get; }
        public AuthorizeClaimsAttribute(params Claims[] claimValues)
        {
            this.claimValues = claimValues;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isInRole = false;

            if (HttpContext.Current.Session["UserClaims"] != null)
            {
                var userClaims = (List<UserClaimsDto>)HttpContext.Current.Session["UserClaims"];

                if (userClaims.Any(_ => _.Name == UtilityHelper.GetEnumDescription(Claims.ManageOrganization) && _.IsApplied))
                    return true;

                foreach (var roleValue in claimValues)
                {
                    if (userClaims != null)
                    {
                        isInRole = userClaims.Any(_ => _.Name == UtilityHelper.GetEnumDescription(roleValue) && _.IsApplied);
                        if (isInRole)
                            break;
                    }
                }
            }

            return isInRole;
        }
    }
}