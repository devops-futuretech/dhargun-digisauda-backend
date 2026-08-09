using Adani.Solution.DTO;
using Adani.Solution.DTO.Enums;
using GMCore.Helper;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using Sys = System.Configuration;

namespace Adani.Solution.MVC.Common
{
    public class Settings
    {
        //Cookie
        public const string CookiePrefix = "qur_";
        public const string CookieUsername = "User";
        public const int SecurityCookieExpiryInDays = 365;

        public const int OptionMaxCount = 10;

        public static int InActiveDays = 1;
        public static int InactiveDaysCount = 100;
        public const int GridOtherColumnWidth = 100;
        public const int GridColumnMaxWidth = 150;
        public const int GridColumnButtonWidth = 120;
        public const int GridColumnMaxWidth250 = 250;
        public const int GridButtonColumnWidth = 50;
        public const int GridFilterColumnWidth = 70;
        public const int GridLevelColumnWidth = 10;
        public static readonly int[] GridPageSizes = { 10, 20, 50, 100, 300, 500 };
        public const int DefaultPageSize = 20;
        public const int MinimumPageSize = 10;
        public const int MandatorySkuPageSize = 1000;
        public const string DateFormat = "dd-MMM-yyyy";
        public const string DateTimeFormat = "dd-MMM-yyyy hh:mm tt";
        public const string DateTimeFormatPicker = "dd-MMM-yyyy HH:mm";
        public const string GridDateFormat = "{0:dd-MMM-yyyy}";
        public const string GridDateTimeFormat = "{0:dd-MMM-yyyy hh:mm tt}";
        public const string GridDateFormatForSalesRegisterAndPendingContracts = "{0:dd/MM/yyyy}";
        public const int PJPFlag = 1;
        public const string DateFormatForImportData = "dd/MM/yyyy";
        public const string DateTimeFormatForImportData = "dd-MMM-yyyy HH:mm";
        public static string[] DateColumns = { "ValidFrom", "ValidTo", "ContractDate" };
        public const string ReportDateFormat = "{0:dd-MMM-yyyy}";
        public const string DefaultDateFormat = "{0:dd-MMM-yyyy}";
        public const int WindowTimeInterval = 2;
        public const int WindowTimeIntervalBiddingWindow = 1;
        public const string GridTimeFormat24 = "{0:HH:mm tt}";
        public const string GridTimeFormat12 = "{0:hh:mm tt}";

        public const string PdfFileContains = "application/pdf";
        public const string ImageFileContains = "image";
        public const string VideoFileContains = "video";
        public const string AcceptedImportFileFormat = ".xlsx";
        public const string AcceptedImageFormat = ".jpg,.png";
        public const string AcceptedVideoFormat = ".flv, .wmv, .mp4";
        public const int ImageFileSize = 20;
        public const int VideoFileSize = 20;
        public const string DefaultImageUrl = "/images/default.png";
        public const string DefaultProfileImageUrl = "/images/DefaultUserProfile.png";
        public const string DefaultImageName = "default.png";

        public const string NextLineBreak = "<br/>";
        public const int VerifyOtpHit = 5;
        public const string ResponseSuccess = "Y77T3XP2B";
        public const string ResponseError = "SXVI7XCEU";
        public const string Response = "response";
        public const string ResponseWebToken = "E6DYES1Q2";
        public const string ResponseMessage = "Message";

        public const string IndianRupeeSymbol = "₹";
        public const string PercentageSymbol = "%";
        public const string CategoryImageFolder = "Category";
        public const string ProductImageFolder = "Product";
        public const string OrderImageFolder = "Order";
        public const string UserImageFolder = "User";
        public static int NumericTextboxMinValue = 1;
        public const string SingleSpace = " ";
        public const string Underscore = " - ";
        public const int ThumbnailHeight = 120;
        public const int ThumbnailWidth = 120;
        public const string ThumbString = "_thumb";
        public const string KeyType = "WebKey";
        public static int CaptchaSringLength = 5;
        public static int DefaultCountryId = 1;
        public static DateTime DefaultDashboardStartDate = new DateTime(DateTime.Now.Year, 1, 1);
        public static DateTime DefaultDashboardEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        public static DateTime DefaultCurrentDate = DateTime.Now;
        public const string DefaultAllString = "All";
        public const int DefaultAllId = -1;
        public const string DefaultDecimalPlaces = "0.00";
        public const string DefaultDecimalPlacesForLabel = "{0:0.00}";
        public const int BulletinImageFileSize = 20;
        public const string FilterOperator = "contains";
        public const string DateFilterOperator = "gte";
        public static FilterType FilterSuggestion = FilterType.Contains;
        public const string UploadToBlobFailed = "rmmE300";
        public const string DefaultNoImageUrl = "/images/default.jpg";
        public const string PriceNumberFormat = "{0:n2}";
        public const string DecimalFormat4 = "{0:0.0000}";
        public const string DecimalFormat3 = "{0:0.000}";
        public const string DecimalFormat2 = "{0:0.00}";       
        public const string Yes = "Yes";
        public const string ComplaintForm = "Complaint form";
        public const string UnderstandingForm = "Understanding form ";
        public const string CompanyName = "AWL Agri business.";
        public const string Active = "1";



        public static string[] BulletinFileFormatArray =
        {
            ".png",
            ".jpg",
            ".jpeg",
            ".mp4",
            ".pdf"
        };

        public static string[] SupportIssueFileFormatArray =
        {
            ".png",
            ".jpg",
            ".jpeg",
        };



        public static Uri ApiUrl => new Uri(Sys.ConfigurationManager.AppSettings["apiurl"]);



        public static ClaimsPrincipal GetIdentity(AuthorizeOutputDto userProfileDto)
        {
            var identity = new ClaimsIdentity();

            if (!string.IsNullOrWhiteSpace(userProfileDto.RoleId))
            {
                identity.AddClaim(new Claim("RoleId", userProfileDto.RoleId.ToString()));

                //foreach (var role in userProfileDto.RoleId.Split(',').Select(int.Parse))
                //{
                //    identity.AddClaim(new Claim(ClaimTypes.Role, EnumHelper.GetEnumDescription((Role)role)));
                //}


            }

            identity.AddClaim(new Claim("UserId", userProfileDto.UserId.ToString()));
            identity.AddClaim(new Claim("Username", userProfileDto.Name));
            identity.AddClaim(new Claim("ProfileName", !string.IsNullOrEmpty(userProfileDto.ProfileName) ? userProfileDto.ProfileName : string.Empty));
            identity.AddClaim(new Claim("RoleId", userProfileDto.RoleId.ToString()));
            identity.AddClaim(new Claim("RoleTypeId", userProfileDto.RoleTypeId.ToString()));
            identity.AddClaim(new Claim("VerticalId", userProfileDto.VerticalId.ToString()));
            identity.AddClaim(new Claim("HeadquartersId", userProfileDto.HeadquartersId.ToString()));
            identity.AddClaim(new Claim("OrganizationReportingToId", userProfileDto.OrganizationReportingToId.ToString()));
            //identity.AddClaim(new Claim("ProfilePath", userProfileDto.ProfilePath.ToString()));
            return new ClaimsPrincipal(identity);
        }

        public AuthorizeOutputDto GetUserProfile(HttpRequestBase httpContext)
        {
            AuthorizeOutputDto userProfileDto = null;
            var cookieName = FormsAuthentication.FormsCookieName;
            if (httpContext.IsAuthenticated && httpContext.Cookies != null && httpContext.Cookies[cookieName] != null)
            {
                var authCookie = httpContext.Cookies[cookieName];
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    userProfileDto = JsonConvert.DeserializeObject<AuthorizeOutputDto>(authTicket.UserData);
                }
            }
            return userProfileDto;
        }

        public static AuthorizeOutputDto GetStaticUserProfile(HttpRequestBase httpContext)
        {
            AuthorizeOutputDto userProfileDto = null;
            var cookieName = FormsAuthentication.FormsCookieName;
            if (httpContext.IsAuthenticated && httpContext.Cookies != null && httpContext.Cookies[cookieName] != null)
            {
                var authCookie = httpContext.Cookies[cookieName];
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    userProfileDto = JsonConvert.DeserializeObject<AuthorizeOutputDto>(authTicket.UserData);
                }
            }
            return userProfileDto;
        }

        public static List<int> GetReports(System.Security.Principal.IIdentity identity)
        {
            var reports = new List<int>();
            var claimIdentity = (ClaimsIdentity)identity;
            var lstReport = claimIdentity.FindFirst("ReportId")?.Value;
            if (!string.IsNullOrWhiteSpace(lstReport))
                reports = lstReport.Split(',').Select(int.Parse).ToList();
            return reports;
        }

        public static string GetPriceWithCurrencyForLable(double? amount)
        {
            string amountindecimal = String.Format(DefaultDecimalPlacesForLabel, amount);
            var currencyWithPrice = string.Concat(IndianRupeeSymbol, " ", amountindecimal);
            return HttpUtility.HtmlDecode(currencyWithPrice);
        }

        public static string GetPriceWithCurrencyForLable(string amount)
        {
            amount = ConvertToTwoDecimalValues(amount);
            if (string.IsNullOrEmpty(amount)) return string.Empty;
            var currencyWithPrice = string.Concat(IndianRupeeSymbol, " ", amount);
            return HttpUtility.HtmlDecode(currencyWithPrice);
        }

        public static string GetPriceWithPercentageForLable(string amount)
        {
            amount = ConvertToTwoDecimalValues(amount);
            if (string.IsNullOrEmpty(amount)) return string.Empty;
            var currencyWithPrice = string.Concat(amount, " ", PercentageSymbol);
            return HttpUtility.HtmlDecode(currencyWithPrice);
        }

        private static string ConvertToTwoDecimalValues(string amount)
        {
            string concatStr = string.Empty;
            bool isSubGridColumn = false;
            if (!string.IsNullOrEmpty(amount))
            {
                string[] chars = new string[] { "=", "#", @"\" };
                for (int i = 0; i < chars.Length; i++)
                {
                    if (amount.Contains(chars[i]))
                    {
                        if (chars[i] == @"\")
                        {
                            isSubGridColumn = true;
                        }
                        amount = amount.Replace(chars[i], "");
                    }
                }

                if (isSubGridColumn)
                {
                    concatStr = "\\#= kendo.toString(" + amount + ", '" + DefaultDecimalPlaces + "')\\#";
                }
                else
                {
                    concatStr = "#= kendo.toString(" + amount + ", '" + DefaultDecimalPlaces + "')#";
                }
            }
            return concatStr;
        }

        public static string GetMediaPath(int contentTypeId, string fileName)
        {
            var folderName = Enum.GetName(typeof(ContentType), contentTypeId);

            var filePath = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                if (ConfigHelper.IsThumbnail)
                {
                    var fileDetaills = fileName.Split('.');
                    if (fileDetaills.Count() > 1)
                    {
                        fileName = string.Concat(fileDetaills[0], ConfigHelper.ThumbnailSuffix, ConfigHelper.ImageExtension);
                    }
                }
                if (System.IO.File.Exists(Path.Combine(HttpContext.Current.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName)))
                {
                    filePath = Path.Combine("..", ConfigHelper.UploadMediaPath, folderName, fileName);
                }
            }
            return filePath;
        }

        public static string GetMediaPathForCallRecordingImage(int contentTypeId, string fileName)
        {
            var folderName = Enum.GetName(typeof(PageType), contentTypeId);

            var filePath = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                if (ConfigHelper.IsThumbnail)
                {
                    var fileDetaills = fileName.Split('.');
                    if (fileDetaills.Count() > 1)
                    {
                        fileName = string.Concat(fileDetaills[0], ConfigHelper.ThumbnailSuffix, ConfigHelper.ImageExtension);
                    }
                }
                if (System.IO.File.Exists(Path.Combine(HttpContext.Current.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName)))
                {
                    filePath = Path.Combine("..", ConfigHelper.UploadMediaPath, folderName, fileName);
                }
            }
            return filePath;
        }

        public static string GetMediaPathForBulkDownload(int contentTypeId, string fileName)
        {
            var folderName = Enum.GetName(typeof(PageType), contentTypeId);

            var filePath = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                if (ConfigHelper.IsThumbnail)
                {
                    var fileDetaills = fileName.Split('.');
                    if (fileDetaills.Count() > 1)
                    {
                        fileName = string.Concat(fileDetaills[0], ConfigHelper.ThumbnailSuffix, ConfigHelper.ImageExtension);
                    }
                }
                if (System.IO.File.Exists(Path.Combine(HttpContext.Current.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName)))
                {
                    filePath = Path.Combine(HttpContext.Current.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName);
                }
            }
            return filePath;
        }

        public static string GetMediaPathForExcelCallRecording(int contentTypeId, string fileName)
        {
            var folderName = Enum.GetName(typeof(PageType), contentTypeId);

            var filePath = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                if (ConfigHelper.IsThumbnail)
                {
                    var fileDetaills = fileName.Split('.');
                    if (fileDetaills.Count() > 1)
                    {
                        fileName = string.Concat(fileDetaills[0], ConfigHelper.ThumbnailSuffix, ConfigHelper.ImageExtension);
                    }
                }
                if (System.IO.File.Exists(Path.Combine(HttpContext.Current.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName)))
                {
                    filePath = Path.Combine(ConfigHelper.WebsiteBaseUrlPath, folderName, fileName);
                }
            }
            return filePath;
        }

        public static string GetAPIMediaPath(string fileName)
        {
            var filePath = Path.Combine(ApiUrl.AbsoluteUri, fileName);
            return filePath;
        }

        public static string GetAPIMediaPathFromFolder(string fileName, int pageId)
        {
            var filePath = DefaultImageUrl;
            var folderName = Enum.GetName(typeof(PageType), pageId);
            if (!string.IsNullOrEmpty(fileName))
                filePath = ConfigHelper.ApiBaseUrlPath + Path.Combine(ConfigHelper.UploadAttachment, folderName, fileName);

            return filePath;
        }
        public static Boolean ValidateIMEI(string IMEI)
        {
            if (IMEI.Length != 15)
                return true;
            else
            {
                Int32[] PosIMEI = new Int32[15];
                for (int innlop = 0; innlop < 15; innlop++)
                {
                    PosIMEI[innlop] = Convert.ToInt32(IMEI.Substring(innlop, 1));
                    if (innlop % 2 != 0) PosIMEI[innlop] = PosIMEI[innlop] * 2;
                    while (PosIMEI[innlop] > 9) PosIMEI[innlop] = (PosIMEI[innlop] % 10) + (PosIMEI[innlop] / 10);
                }

                Int32 Totalval = 0;
                foreach (Int32 v in PosIMEI) Totalval += v;
                if (0 == Totalval % 10)
                    return false;
                else
                    return true;
            }

        }

        public static Boolean ValidateAddrMac(string addrMac)
        {
            addrMac = addrMac.Replace(" ", "").Replace(":", "").Replace("-", "");
            Regex r = new Regex("^[a-fA-F0-9]{12}$");

            if (r.IsMatch(addrMac))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //public static string GetBulletinMediaUrl(string blobName)
        //{
        //    var blobUri = string.Empty;
        //    var mediaClient = new MediaClient();
        //    var mediaDto = new MediaDto
        //    {
        //        BlobName = blobName,
        //        ProfileOrMediaContainer = ConfigHelper.MediaContainer
        //    };
        //    blobUri = mediaClient.GetBlobUri(mediaDto);
        //    return blobUri;
        //}

        //public static string GetUserMediaUrl(string blobName)
        //{
        //    var blobUri = string.Empty;
        //    var encryptedBlobName = EncryptDecryptHelper.Encrypt(blobName, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
        //    var mediaClient = new MediaClient();
        //    var mediaDto = new MediaDto
        //    {
        //        BlobName = encryptedBlobName,
        //        ProfileOrMediaContainer = ConfigHelper.UserContainer
        //    };
        //    blobUri = mediaClient.GetBlobUri(mediaDto);
        //    if (!string.IsNullOrEmpty(blobUri))
        //        return blobUri;
        //    else
        //        return Settings.DefaultProfileImageUrl;
        //}

        public static decimal ConvertSringToDecimal(string value)
        {
            decimal number = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    number = Convert.ToDecimal(Convert.ToDecimal(value).ToString("0.00"));
                    return number;
                }
            }
            catch (Exception) { }

            return number;
        }

        public static decimal ConvertSringToLaks(string value)
        {
            decimal number = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    number = Convert.ToDecimal(Convert.ToDecimal(value).ToString("0.00")) * 100000;
                    return number;
                }
            }
            catch (Exception) { }

            return number;
        }

        public static int ConvertSringToInt(string value)
        {
            int number = 0;
            if (Int32.TryParse(value, out number))
                return number;
            else
                return number;
        }

        public static string CheckMobileNumber(string value)
        {
            string mobilenumber = value;
            long number = 0;
            bool canConvert = long.TryParse(value, out number);
            if (canConvert)
                return mobilenumber;
            else
            {
                if (value.IndexOf('-') > -1)
                    return mobilenumber;
                else
                    return string.Empty;
            }
        }


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

        public static string[] ConvertCommaSeparatedStringToStringArray(string commaSeparatedString)
        {
            var stringList = new List<string>();
            if (!string.IsNullOrEmpty(commaSeparatedString))
            {
                stringList = commaSeparatedString.TrimEnd(',').Split(',').Where(s => !string.IsNullOrEmpty(s)).ToList();
            }
            return stringList.ToArray();
        }

        public static string DateFormats(DateTime? dateTime, string format)
        {
            if (dateTime.HasValue)
            {
                if (string.IsNullOrEmpty(format)) format = ReportDateFormat;
                return string.Format(format, dateTime);
            }
            return string.Empty;
        }

        public static List<T> DeserializeObject<T>(string data, JsonSerializerSettings settings)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var base64EncodedBytes = Convert.FromBase64String(data);
                var json = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                if (settings != null)
                    return JsonConvert.DeserializeObject<List<T>>(json, settings);
                else
                    return JsonConvert.DeserializeObject<List<T>>(json);
            }
            return new List<T>();
        }

        public static string SAPExcInvoke(string sapTypeId)
        {
            var exePath = ConfigHelper.SAPEXEPath; 
            var message = string.Concat(sapTypeId,"", " sync successfully done");
            sapTypeId = sapTypeId.Replace(" ", "");
            var filePath = string.Concat(exePath, "\\Adani.Solution.Console.exe");
            var myProcess = new Process();
            myProcess.StartInfo.FileName = filePath;
            myProcess.StartInfo.Arguments = $"{sapTypeId}";                   
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.Start();
            myProcess.WaitForExit();               
            return message;
        }
    }
}