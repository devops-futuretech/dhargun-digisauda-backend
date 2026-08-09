using System;
using System.Configuration;

namespace Adani.Solution.MVC.Common
{
    public static class ConfigHelper
    {
        public static bool IsThumnailImageCreation => Convert.ToBoolean(ConfigurationManager.AppSettings["IsThumbnailImageCreation"]);
        public static string UploadMediaPath => ConfigurationManager.AppSettings["UploadMediaPath"];
        public static string ImageExtension => ConfigurationManager.AppSettings["ImageExtension"];
        public static string WebKey => ConfigurationManager.AppSettings["WebKey"];
        public static string KendoThemeName => ConfigurationManager.AppSettings["KendoThemeName"];
        public static string Version => ConfigurationManager.AppSettings["Version"];
        public static string WebsiteUrl => ConfigurationManager.AppSettings["WebsiteUrl"];
        public static int ImageCount => Convert.ToInt32(ConfigurationManager.AppSettings["ImageCount"]);

        public static int AllowMaximumBulletinMediaCount => Convert.ToInt32(ConfigurationManager.AppSettings["AllowMaximumBulletinMediaCount"]);
        public static string UploadBulletinPath => ConfigurationManager.AppSettings["UploadBulletinPath"];
        public static string BlobSuffixForUserProfile => ConfigurationManager.AppSettings["BlobSuffixForUserProfile"];
        public static string ThumbnailSuffix => ConfigurationManager.AppSettings["ThumbnailSuffix"];
        public static string BlobSuffixForBulletin => ConfigurationManager.AppSettings["BlobSuffixForBulletin"];
        public static string MediaContainer => ConfigurationManager.AppSettings["MediaContainer"];
        public static string UserContainer => ConfigurationManager.AppSettings["UserContainer"];
        public static string StorageConnectionString => ConfigurationManager.AppSettings["StorageConnectionString"];
        public static bool IsThumbnail => Convert.ToBoolean(ConfigurationManager.AppSettings["IsThumbnail"]);
        public static string SPConnectionString => ConfigurationManager.AppSettings["SPConnectionString"];

        public static bool IsSapDataUpdate => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSapDataUpdate"]);
        public static int MaxSelectedOneItemForMultiselect => Convert.ToInt32(ConfigurationManager.AppSettings["MaxSelectedOneItemForMultiselect"]);
        public static int MaxSelectedItemForDistrict => Convert.ToInt32(ConfigurationManager.AppSettings["MaxSelectedItemForDistrict"]);
        public static int MaxSelectedDefaultItemForMultiselect => Convert.ToInt32(ConfigurationManager.AppSettings["MaxSelectedDefaultItemForMultiselect"]);

        public static string SAPEXEPath => ConfigurationManager.AppSettings["SAPEXEPath"];
        public static int RecordCountForExcelSheet => Convert.ToInt32(ConfigurationManager.AppSettings["RecordCountForExcelSheet"]);

        public static string ApiBaseUrlPath => ConfigurationManager.AppSettings["AttachmentApiUrl"];
        public static string UploadAttachment => ConfigurationManager.AppSettings["UploadAttachment"];
        public static string WebsiteBaseUrlPath => ConfigurationManager.AppSettings["AttachmentWebsiteUrl"];
        public static string AudioFileInExcelUrl => ConfigurationManager.AppSettings["AudioFileInExcelUrl"];
        public static int DateBeforeThreeMonths => Convert.ToInt32(ConfigurationManager.AppSettings["DateBeforeThreeMonths"]);

        public static bool ContractDashboard => Convert.ToBoolean(ConfigurationManager.AppSettings["ContractDashboard"]);
        public static bool SalesOrderDashboard => Convert.ToBoolean(ConfigurationManager.AppSettings["SalesOrderDashboard"]);
        public static bool DueDashboard => Convert.ToBoolean(ConfigurationManager.AppSettings["DueDashboard"]);
        public static string DashboardCardAPITime => Convert.ToString(ConfigurationManager.AppSettings["DashboardCardAPITime"]);

        //VehicleTrackingList
        public static string VehicleTrackStatusLoginAPI => Convert.ToString(ConfigurationManager.AppSettings["VehicleTrackStatusLoginAPI"]);
        public static string VehicleStatusDataAPI => Convert.ToString(ConfigurationManager.AppSettings["VehicleStatusDataAPI"]);
        public static string VehicleStatusUserName => Convert.ToString(ConfigurationManager.AppSettings["VehicleStatusUserName"]);
        public static string VehicleStatusPassword => Convert.ToString(ConfigurationManager.AppSettings["VehicleStatusPassword"]);
        public static string GoogleAnalyticsKeyFilePath => Convert.ToString(ConfigurationManager.AppSettings["GoogleAnalyticsKeyFilePath"]);
        public static string GoogleAnalyticsWebPropertyId => Convert.ToString(ConfigurationManager.AppSettings["GoogleAnalyticsWebPropertyId"]);
        public static string GoogleAnalyticsMobilePropertyId => Convert.ToString(ConfigurationManager.AppSettings["GoogleAnalyticsMobilePropertyId"]);
        public static long GoogleAnalyticsPreviousUserCount => Convert.ToInt32(ConfigurationManager.AppSettings["GoogleAnalyticsPreviousUserCount"]);
        public static long GoogleAnalyticsPreviousMobileUserCount => Convert.ToInt32(ConfigurationManager.AppSettings["GoogleAnalyticsPreviousMobileUserCount"]);
        public static long GoogleAnalyticsDataDaysInterval => Convert.ToInt32(ConfigurationManager.AppSettings["GoogleAnalyticsDataDaysInterval"]);
        public static string PushNotificationConfigFileName => Convert.ToString(ConfigurationManager.AppSettings["PushNotificationConfigFileName"]);
        public static string Scopes => Convert.ToString(ConfigurationManager.AppSettings["Scopes"]);
        public static string FCMUrl => Convert.ToString(ConfigurationManager.AppSettings["FCMUrl"]);


    }
}