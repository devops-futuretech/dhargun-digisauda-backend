using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using GMCore.Logger;
using Newtonsoft.Json;
using GMCore.Helper;
using Adani.Solution.DTO.Enums;
using Adani.Solution.MVC.Common;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Kendo.Mvc.UI;
using OfficeOpenXml;
using System.Drawing;
using System.Web.Hosting;

namespace Adani.Solution.MVC.Controllers
{
    [HandleCustomError]
    public class BaseController : Controller
    {
        private readonly ILogger _logger = Logging.GetLogger("BaseController");

        public string AccessToken => System.Web.HttpContext.Current.Request.Headers.Get("Authorization");

        public bool IsAdmin => ControllerContext.HttpContext.User.IsInRole(EnumHelper.GetEnumDescription(Role.Admin));
        public bool IsZonalHead => ControllerContext.HttpContext.User.IsInRole(EnumHelper.GetEnumDescription(Role.ZonalTrader));
        public bool IsDealer => ControllerContext.HttpContext.User.IsInRole(EnumHelper.GetEnumDescription(Role.Dealer));
        //public bool IsPrimaryUser => ControllerContext.HttpContext.User.IsInRole(EnumHelper.GetEnumDescription(Role.DealerPrimaryUser));
        //public bool IsSystem => ControllerContext.HttpContext.User.IsInRole(EnumHelper.GetEnumDescription(Role.System));

        public int UserId
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return Convert.ToInt32(identity.FindFirst("UserId")?.Value);
            }
        }

        public string UserName
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return identity.FindFirst("UserName")?.Value;
            }
        }

        public int RoleId
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return Convert.ToInt32(identity.FindFirst("RoleId")?.Value);
            }
        }

        public int RoleTypeId
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return Convert.ToInt32(identity.FindFirst("RoleTypeId")?.Value);
            }
        }

        public string ProfileName
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return identity.FindFirst("ProfileName")?.Value;
            }
        }

        public long VerticalId
        {
            get
            {
                //var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                //return Convert.ToInt32(identity.FindFirst("VerticalId")?.Value);
                return Convert.ToInt32(0);
            }
        }
        public long HeadquartersId
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return Convert.ToInt32(identity.FindFirst("HeadquartersId")?.Value);
            }
        }

        public long OrganizationReportingToId
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return Convert.ToInt32(identity.FindFirst("OrganizationReportingToId")?.Value);
            }
        }

        /// <summary>
        /// Dummy method to avoid dependency injection issues calling User action method
        /// </summary>
        /// <returns></returns>
        public ActionResult user()
        {
            return new EmptyResult();
        }

        public HttpResponseMessage GetAsync(string functionUrl)
        {
            _logger.Debug("GetAsync Call " + functionUrl);
            using (var client = new HttpClient())
            {
                SetClientTimeout(client);
                if (AccessToken != null)
                {
                    SetBearerToken(AccessToken, client);
                }

                var requestUri = new Uri(Settings.ApiUrl, functionUrl);
                var responseMessage = client.GetAsync(requestUri).Result;
                //RefreshAuthCookie(responseMessage);

                return ValidateResponseMessage(responseMessage);
            }
        }

        public HttpResponseMessage PostAsync(string functionUrl, object model)
        {
            _logger.Info("PostAsync Call " + functionUrl);

            using (var client = new HttpClient())
            {
                SetClientTimeout(client);
                if (AccessToken != null)
                {
                    SetBearerToken(AccessToken, client);
                }

                var webApiUrl = new Uri(Settings.ApiUrl, functionUrl);
                var responseMessage = client.PostAsJsonAsync(webApiUrl, model).Result;
                //RefreshAuthCookie(responseMessage);

                return ValidateResponseMessage(responseMessage);
            }

        }
        public string SaveExcelFileToPath(ExcelPackage excelPackage)
        {
            //_methodName = "SaveExcelFileToPath";
            //_logger.Info($"{ServiceName} Controller-Method {_methodName}");

            string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
            string fileGuid = Guid.NewGuid().ToString();
            string guidFileName = fileGuid + ".xlsx";
            string savePath = Path.Combine(serverFoloderPath, guidFileName);

            if (System.IO.File.Exists(savePath))
            {
                System.IO.File.Delete(savePath);
                using (Stream stream = System.IO.File.Create(savePath))
                {
                    excelPackage.SaveAs(stream);
                }
            }
            else
            {
                using (Stream stream = System.IO.File.Create(savePath))
                {
                    excelPackage.SaveAs(stream);
                }
            }
            return guidFileName;
        }
        public HttpResponseMessage PostAsync(string functionUrl, object model, string webToken)
        {
            _logger.Info("PostAsync Call " + functionUrl);

            using (var client = new HttpClient())
            {
                SetClientTimeout(client);
                if (!string.IsNullOrEmpty(webToken))
                {
                    SetBearerToken(webToken, client);
                }

                var webApiUrl = new Uri(Settings.ApiUrl, functionUrl);
                var responseMessage = client.PostAsJsonAsync(webApiUrl, model).Result;
                //RefreshAuthCookie(responseMessage);

                return ValidateResponseMessage(responseMessage);
            }

        }

        public HttpResponseMessage PostFileAsync(string functionUrl, string fileName, Stream fileStream)
        {
            using (var client = new HttpClient())
            {
                SetClientTimeout(client);
                client.BaseAddress = Settings.ApiUrl;

                if (AccessToken != null)
                {
                    SetBearerToken(AccessToken, client);
                }

                var content = new MultipartFormDataContent { { new StreamContent(fileStream), "file", fileName } };

                var message = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = content,
                    RequestUri = new Uri(Settings.ApiUrl, functionUrl)
                };

                var responseMessage = client.SendAsync(message).Result;
                //RefreshAuthCookie(responseMessage);

                return ValidateResponseMessage(responseMessage);
            }
        }

        public HttpResponseMessage PutAsync(string functionUrl, object model)
        {
            _logger.Info("PutAsync Call " + functionUrl);
            using (var client = new HttpClient())
            {
                SetClientTimeout(client);
                client.BaseAddress = Settings.ApiUrl;

                if (AccessToken != null)
                {
                    SetBearerToken(AccessToken, client);
                }

                var webApiUrl = new Uri(Settings.ApiUrl, functionUrl).ToString();
                var responseMessage = client.PutAsJsonAsync(webApiUrl, model).Result;
                //RefreshAuthCookie(responseMessage);

                return ValidateResponseMessage(responseMessage);
            }

        }

        public HttpResponseMessage PutAsync(string functionUrl, object model, string webToken)
        {
            _logger.Info("PutAsync Call " + functionUrl);
            using (var client = new HttpClient())
            {
                SetClientTimeout(client);
                client.BaseAddress = Settings.ApiUrl;

                if (!string.IsNullOrEmpty(webToken))
                {
                    SetBearerToken(webToken, client);
                }

                var webApiUrl = new Uri(Settings.ApiUrl, functionUrl).ToString();
                var responseMessage = client.PutAsJsonAsync(webApiUrl, model).Result;
                //RefreshAuthCookie(responseMessage);

                return ValidateResponseMessage(responseMessage);
            }

        }



        public HttpResponseMessage DeleteAsync(string functionUrl)
        {
            _logger.Info("DeleteAsync Call " + functionUrl);

            using (var client = new HttpClient())
            {
                SetClientTimeout(client);
                client.BaseAddress = Settings.ApiUrl;
                if (AccessToken != null)
                {
                    SetBearerToken(AccessToken, client);
                }

                var webApiUrl = new Uri(Settings.ApiUrl, functionUrl);
                var responseMessage = client.DeleteAsync(webApiUrl).Result;
                //RefreshAuthCookie(responseMessage);

                return ValidateResponseMessage(responseMessage);
            }
        }
        private static void SetBearerToken(string accessToken, HttpClient client)
        {
            // Add the Token header in the request message
            var header = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = header;
        }

        private HttpResponseMessage ValidateResponseMessage(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                return responseMessage;
            }
            _logger.Debug("Response Message " + responseMessage);
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                FormsAuthentication.SignOut();
                var currentContext = System.Web.HttpContext.Current;
                currentContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                var context = new RequestContext(new HttpContextWrapper(currentContext), new RouteData());
                var urlHelper = new UrlHelper(context);
                var url = urlHelper.Action("Index", "Home");//Also update in AccountController & JsonLogOff Action
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    System.Web.HttpContext.Current.Response.Redirect(urlHelper.Action("Index", "Home", new { url = url ?? string.Empty }));
                }
                else if (url != null)
                {
                    System.Web.HttpContext.Current.Response.Redirect(url);
                }
            }
            if (responseMessage.StatusCode == HttpStatusCode.Conflict)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var data = "{ \"$id\":\"1\",\"Message\":" + responseData + ",\"IsSuccess\":false,\"ErrorCode\":\"tngsqE409\",\"Response\":\"\"}";
                responseMessage.Content = new StringContent(data, Encoding.UTF8, "application/json");
                return responseMessage;
            }
            //else if (responseMessage.StatusCode == HttpStatusCode.InternalServerError)
            //{
            //    var context = new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData());
            //    var urlHelper = new UrlHelper(context);
            //    var url = urlHelper.Action(Settings.ErrorAction, Settings.SharedController);//Also update in AccountController & JsonError Action
            //    if (context.HttpContext.Request.IsAjaxRequest())
            //    {
            //        System.Web.HttpContext.Current.Response.Redirect(urlHelper.Action(Settings.JsonErrorAction, Settings.AccountController, new { url = url ?? string.Empty }));
            //    }
            //    else if (url != null)
            //    {
            //        System.Web.HttpContext.Current.Response.Redirect(url);
            //    }
            //}
            return responseMessage;
        }
        // Reads the token from the authorization header on the incoming request
        private static bool TryRetrieveToken(HttpResponseMessage response, out string token)
        {
            token = null;

            if (!response.Headers.Contains("Authorization"))
            {
                return false;
            }

            var authzHeader = response.Headers.GetValues("Authorization").First();

            // Verify Authorization header contains 'Bearer' scheme
            token = authzHeader.StartsWith("Bearer ") ? authzHeader.Split(' ')[1] : null;

            if (null == token)
            {
                return false;
            }

            return true;
        }

        //private void RefreshAuthCookie(HttpResponseMessage responseMessage)
        //{
        //    string token;
        //    TryRetrieveToken(responseMessage, out token);
        //    if (token != null)
        //    {
        //        string cookieName = FormsAuthentication.FormsCookieName;
        //        var authCookie = System.Web.HttpContext.Current.Request.Cookies[cookieName];
        //        if (authCookie != null)
        //        {
        //            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //            var userProfile = JsonConvert.DeserializeObject<UserDto>(authTicket.UserData);
        //            string headerVal = token;
        //            userProfile.WebToken = headerVal;
        //            var authCookieNew = CreateAuthCookie(authTicket.Name, userProfile);
        //            System.Web.HttpContext.Current.Response.Cookies.Remove(authCookie.Name);
        //            System.Web.HttpContext.Current.Response.AppendCookie(authCookieNew);

        //        }

        //    }
        //}
        protected HttpCookie CreateAuthCookie(string authTicketName, AuthorizeOutputDto userProfileDto)
        {
            var authTicket = new FormsAuthenticationTicket(
                1,
                authTicketName,
                DateTime.Now,
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                true,
                JsonConvert.SerializeObject(userProfileDto), FormsAuthentication.FormsCookiePath
                );
            var authCookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(authTicket)
                )
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes)
            };
            return authCookie;
        }

        private void SetClientTimeout(HttpClient client)
        {
            client.Timeout = new TimeSpan(10, 0, 0);
        }

        public dynamic ConvertUTCToIndiaTime(DateTime? dateTime)
        {
            DateTime? convertedDate = null;
            if (dateTime != null)
            {
                convertedDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(Convert.ToDateTime(dateTime), DateTimeKind.Unspecified), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            }
            return convertedDate;
        }

        /// <summary>
        /// Save to excel file in specified path
        /// </summary>
        /// <param name="excelPackage"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public JsonResult SaveExcelFileToPath(OfficeOpenXml.ExcelPackage excelPackage, string fileName)
        {
            string serverFoloderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/FinalPriceDownload/");
            string guidFileName = Guid.NewGuid().ToString() + ".xlsx";
            string savePath = Path.Combine(serverFoloderPath, guidFileName);

            if (System.IO.File.Exists(savePath))
            {
                System.IO.File.Delete(savePath);
                using (Stream stream = System.IO.File.Create(savePath))
                {
                    excelPackage.SaveAs(stream);
                }
            }
            else
            {
                using (Stream stream = System.IO.File.Create(savePath))
                {
                    excelPackage.SaveAs(stream);
                }
            }
            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Excel file download
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExcelDownload(string fileGuid, string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/FinalPriceDownload/" + fileGuid);
            byte[] byteArray = null;
            if (System.IO.File.Exists(path))
            {
                byteArray = System.IO.File.ReadAllBytes(path);
                System.IO.File.Delete(path);
            }
            return File(byteArray, "application/vnd.ms-excel", fileName);
        }

        [AllowAnonymous]
        public ActionResult AudioFileDownload(string fileName,string fileDownloadName)
        {
            byte[] byteArray = null;
            var PathExtension = Path.GetExtension(fileName);
            var fileDownload = fileDownloadName + PathExtension;
            var folderName = UtilityHelper.GetEnumDescription(PageType.AudioFiles);
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/UploadMedias/" + folderName + "/" + fileName);
            if (System.IO.File.Exists(path))
            {
                byteArray = System.IO.File.ReadAllBytes(path);
            }
            return File(byteArray, System.Net.Mime.MediaTypeNames.Application.Octet, fileDownload);
        }
 

       public ActionResult ImageFilesDownload(string fileName)
        {
            byte[] byteArray = null;
            var PathExtension = Path.GetExtension(fileName);
            var fileDownloadName = "Image" + PathExtension;
            var folderName = UtilityHelper.GetEnumDescription(PageType.ImagesSaudaMappingwithCallRecording);
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/UploadMedias/" + folderName + "/" + fileName);
            if (System.IO.File.Exists(path))
            {
                byteArray = System.IO.File.ReadAllBytes(path);
            }
            return File(byteArray, System.Net.Mime.MediaTypeNames.Application.Octet, fileDownloadName);
        }

        public KendoGridResult GridResultInputDto(DataSourceRequest request, bool isToReturnInactiveData)
        {
            request.Filters = Utility.ToFilterDescriptor(request.Filters);
            return new KendoGridResult() { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData,DataSourceRequest = request };
        }

        public KendoGridResult KendoGridResultWithId(DataSourceRequest request, long id, bool isToReturnInactiveData)
        {
            request.Filters = Utility.ToFilterDescriptor(request.Filters);
            return new KendoGridResult() { Id = id, LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, /*VerticalId = VerticalId,*/ DataSourceRequest = request };
        }

        public ExcelRange GetExcelTitle(ExcelRange cell, string title, int level = 0)
        {
            cell.Value = title ?? string.Empty;

            cell.Style.Border.Top.Style =
                cell.Style.Border.Left.Style =
                    cell.Style.Border.Right.Style = cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Color.SetColor(Color.White);
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
            if (level > 0)
            {
                cell.Style.Fill.BackgroundColor.SetColor(Color.Green);
            }
            return cell;
        }

        public ExcelRange GetExcelContent(ExcelRange cell, string text, int align = 0)
        {
            cell.Value = text ?? string.Empty;

            cell.Style.Border.Top.Style =
                cell.Style.Border.Left.Style =
                    cell.Style.Border.Right.Style = cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            if (align == 1)//align right
            {
                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            }

            return cell;
        }
    }
}