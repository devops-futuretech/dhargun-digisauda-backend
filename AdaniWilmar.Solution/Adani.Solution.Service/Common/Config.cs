using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMCore.Helper;

namespace Adani.Solution.Service.Common
{
    public static class Config
    {
        public static int DefaultSaudaValidity = UtilityHelper.IntTryToParse(ConfigurationManager.AppSettings["DefaultSaudaValidity"]);
        public static string WebSiteUrl = ConfigurationManager.AppSettings["WebsiteUrl"];
        public static string PriceNotifyConfigurationFlag = ConfigurationManager.AppSettings["PriceNotifyConfigurationFlag"];
        public static string DBConnectionString = ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString;
        public static long MaxFileSize = UtilityHelper.LongTryToParse(ConfigurationManager.AppSettings["MaxFileSize"]);
        public static int NotificationMaxDays = UtilityHelper.IntTryToParse(ConfigurationManager.AppSettings["NotificationMaxDays"]);


        public static string SmtpHostServerName = ConfigurationManager.AppSettings["SmtpHostServerName"];
        public static string SmtpNetworkCredentialUserName = ConfigurationManager.AppSettings["SmtpNetworkCredentialUserName"];
        public static string SmtpNetworkCredentialPassword = ConfigurationManager.AppSettings["SmtpNetworkCredentialPassword"];
        public static string SmtpFromMailAddress = ConfigurationManager.AppSettings["SmtpFromMailAddress"];
        public static bool SmtpEnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
        public static readonly bool AwsEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSEmail"].ToString());
        public static int SmtpNetworkCredentialPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpNetworkCredentialPort"]);
        public const string EmailSendError = "Email Send Error";

        public static bool IsFinalPriceGenerateOld = Convert.ToBoolean(ConfigurationManager.AppSettings["IsFinalPriceGenerateOld"]);
        public static bool MobileSkuFinalPrice = Convert.ToBoolean(ConfigurationManager.AppSettings["MobileSkuFinalPrice"]);        
        public static int MaximumEmailCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumEmailCount"]);
        public static int MaximumSmsCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumSmsCount"]);
        public static int MaximumPushnotificationCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumPushnotificationCount"]);

        public static int InConditionTakeCount = Convert.ToInt32(ConfigurationManager.AppSettings["InConditionTakeCount"]);

        public static string CCEmail = ConfigurationManager.AppSettings["NewCCEmailForSaudaConversion"];
        public static string WebsitePhysicalPath = ConfigurationManager.AppSettings["WebsitePhysicalPath"];
        public static string MobileImagePath = ConfigurationManager.AppSettings["MobileImagePath"];
        public static int RecordCountForExcelSheet => Convert.ToInt32(ConfigurationManager.AppSettings["RecordCountForExcelSheet"]);
        public static string Company_Code => ConfigurationManager.AppSettings["Company_Code"];
        public static int LastModifiedDate => Convert.ToInt32(ConfigurationManager.AppSettings["LastModifiedDate"]);
        public static string CallRecordingDealerDetailsExpireMins = ConfigurationManager.AppSettings["CallRecordingDealerDetailsExpireMins"];
        public static string IVRNumber = ConfigurationManager.AppSettings["IVRNumber"];
        public static string GoogleAnalyticsKeyFilePath => Convert.ToString(ConfigurationManager.AppSettings["GoogleAnalyticsKeyFilePath"]);
        public static string GoogleAnalyticsWebPropertyId => Convert.ToString(ConfigurationManager.AppSettings["GoogleAnalyticsWebPropertyId"]);
        public static string GoogleAnalyticsMobilePropertyId => Convert.ToString(ConfigurationManager.AppSettings["GoogleAnalyticsMobilePropertyId"]);

    }
}
