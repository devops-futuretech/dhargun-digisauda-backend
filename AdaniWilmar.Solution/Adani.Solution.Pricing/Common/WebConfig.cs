using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Console.Common
{
    public static class WebConfig
    {
        public static Uri ApiUrl => new Uri(ConfigurationManager.AppSettings["apiurl"]);
        public const string WebApiUrlPostVerifyToken = "api/authenticate/verify/console";
        public const string WebApiUrlPostValidateUser = "/api/sap/authorize/user/console";
        public const string WebApiUrlNotification = "api/authenticate/verify/console";
        public const string PendingContractAutoTrigger = "api/sap/pendingcontractautotrigger";
        public const string CustomerLedgerRequestAutoTrigger = "api/sap/customerledgerautotrigger";
        public const string EmployeeRequestActiveUsersAutoTrigger = "api/sap/employeerequestactiveusers";
        public const string EmployeeRequestInActiveUsersAutoTrigger = "api/sap/employeerequestinactiveusers";
        public const string SaudaExpiredNotificationAutoTrigger = "api/sap/saudasxpirednotification";
        public const string OverDueNotificationAutoTrigger = "api/sap/overduenotification";
        public const string LiftingUpdateTrigger = "api/sauda/updateliftingSaudaOrderId";
        public const string WebApiUrlGCPApidata = "api/master/gcpapi/gamificationdashboard";


        public static bool IsPricingBackup => Convert.ToBoolean(ConfigurationManager.AppSettings["IsPricingBackup"]);
        public static bool IsNotification => Convert.ToBoolean(ConfigurationManager.AppSettings["IsNotification"]);
        public static bool IsPendingContractSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsPendingContractSync"]);
        public static bool IsCustomerLedgerRequestSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsCustomerLedgerRequestSync"]);
        public static bool IsCustomerLedgerDeleteSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsCustomerLedgerDeleteSync"]);
        public static bool IsSalesRegisterDeleteSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSalesRegisterDeleteSync"]);
        public static bool IsAuditLogDeleteSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsAuditLogDeleteSync"]);
        public static bool IsUpdateSapSaudaNumberMessageSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsUpdateSapSaudaNumberMessageSync"]);
        
        public static bool IsEmployeeRequestActiveUsersSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsEmployeeRequestActiveUsersSync"]);
        public static bool IsEmployeeRequestInActiveUsersSync => Convert.ToBoolean(ConfigurationManager.AppSettings["IsEmployeeRequestInActiveUsersSync"]);
        public static bool IsSaudaExpiredNotification => Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaudaExpiredNotification"]);
        public static bool IsOverDueNotification => Convert.ToBoolean(ConfigurationManager.AppSettings["IsOverDueNotification"]);


        public const string KeyType = "WebKey";
        public static string WebKey => ConfigurationManager.AppSettings["WebKey"];
        public static string EncryptionKey => ConfigurationManager.AppSettings["EncryptionKey"];
        public static string VectorKey => ConfigurationManager.AppSettings["VectorKey"];

        public static string DBConnectionString = ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString;
        public static string UserNameString = ConfigurationManager.AppSettings["UserName"];
        public static string PasswordString = ConfigurationManager.AppSettings["Password"];

        public static long AuditLogDeleteCount =long.Parse(ConfigurationManager.AppSettings["AuditLogDeleteCount"]);

        public static string IsLogFileDirctoryPathOld => ConfigurationManager.AppSettings["IsLogFileDirctoryPathOld"];
        public static string SapTemplateFileDirctoryPath => ConfigurationManager.AppSettings["SapTemplateFileDirctoryPath"];
        public static string LogBackupFolderPath => ConfigurationManager.AppSettings["LogBackupFolderPath"];
        public static string LogBackupFolderFileName => ConfigurationManager.AppSettings["LogBackupFolderFileName"];
        public static string LogBackupFolderDeleteDays => ConfigurationManager.AppSettings["LogBackupFolderDeleteDays"];
        public static string SapTemplateFileName => ConfigurationManager.AppSettings["SapTemplateFileName"];
        public static string SapTemplateFileType => ConfigurationManager.AppSettings["SapTemplateFileType"];
        public static string IsLogFileDirctoryPathNew => ConfigurationManager.AppSettings["IsLogFileDirctoryPathNew"];
        public static int ThreeDaysBackup => Convert.ToInt32(ConfigurationManager.AppSettings["ThreeDaysBackup"]);
        public static long SuperAdminUserId => Convert.ToInt32(ConfigurationManager.AppSettings["SuperAdminUserId"]);


        //public static readonly bool IsInfoLog = Convert.ToBoolean(ConfigurationManager.AppSettings["IsInfoLog"]);
        //public static readonly bool IsActiveStatusUpdate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsActiveStatusUpdate"]);
        //public static readonly int BiddingWindowTimerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["BiddingWindowTimerInterval"]);
        //public static readonly bool IsDeleteFinalPriceExcelFiles = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDeleteFinalPriceExcelFiles"]);
        //public static readonly int ExcelSheetRecordCount = Convert.ToInt32(ConfigurationManager.AppSettings["ExcelSheetRecordCount"]);
        //public static readonly bool IsWarnLog = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWarnLog"]);
        //public static readonly bool IsDebugLog = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDebugLog"]);
    }    
}
