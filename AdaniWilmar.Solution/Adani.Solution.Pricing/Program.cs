using Adani.Solution.Console.Common;
using Adani.Solution.Console.Services;
using Adani.Solution.Pricing.Services;
using NLog;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Console
{
    class Program
    {
        static Container container;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static string _methodName = string.Empty;
        private static string ServiceName = "Program";

        static Program()
        {
            //container = SimpleInjectorConfguration.Initialize();
        }

        static void Main(string[] args)
        {
            if (true)
            {
                if (true)
                {
                    System.Console.WriteLine("Gamification Dashboard Data Update Service Start");
                    LiftingSaudaOrderUpdateService liftingupdate = new LiftingSaudaOrderUpdateService();
                    liftingupdate.GamificationDashboard();
                    System.Console.WriteLine("Gamification Dashboard Data Update Service End");
                }
                if (args[0].ToString() == "IsPricingBackup")
                {
                    System.Console.WriteLine("Is Pricing Backup");
                    PricingBackupService pricingBackupService = new PricingBackupService();
                    pricingBackupService.InsertForPricingBackup();
                    System.Console.WriteLine("Is Pricing Backup");
                }
                if (args[0].ToString() == "IsNotification")
                {
                    System.Console.WriteLine("Is Notification Start");
                    PricingBackupService pricingBackupService = new PricingBackupService();
                    pricingBackupService.InsertForPricingBackup();
                    System.Console.WriteLine("Is Notification End");
                }

                if (args[0].ToString() == "IsPendingContractSync")
                {
                    System.Console.WriteLine("Is Pending Contract trigger Start");
                    NotificationService notificationService = new NotificationService();
                    notificationService.PendingContractAsync();
                    System.Console.WriteLine("Is  Pending Contract trigger End");
                }

                if (args[0].ToString() == "IsCustomerLedgerRequestSync")
                {
                    System.Console.WriteLine("Is Customer Ledger Start");
                    NotificationService notificationService = new NotificationService();
                    notificationService.CustomerLedgerRequest();
                    System.Console.WriteLine("Is Customer Ledger End");
                }
                if (args[0].ToString() == "IsCustomerLedgerDeleteSync")
                {
                    System.Console.WriteLine("Is Customer Ledger Delete Start");
                    CustomerLedgerDeleteService customerLedgerService = new CustomerLedgerDeleteService();
                    customerLedgerService.DeleteCustomerLedger();
                    System.Console.WriteLine("Is Customer Ledger Delete End");
                }
                if (args[0].ToString() == "IsSalesRegisterDeleteSync")
                {
                    System.Console.WriteLine("Is Sales Register Delete Start");
                    SalesRegisterDeleteService customerLedgerService = new SalesRegisterDeleteService();
                    customerLedgerService.DeleteSalesRegister();
                    System.Console.WriteLine("Is Sales Register Delete End");
                }

                if (args[0].ToString() == "IsEmployeeRequestActiveUsersSync")
                {
                    System.Console.WriteLine("Is Employee Request Active Users Start");
                    NotificationService notificationService = new NotificationService();
                    notificationService.EmployeeRequestActiveUsers();
                    System.Console.WriteLine("Is Employee Request Active Users End");
                }

                if (args[0].ToString() == "IsEmployeeRequestInActiveUsersSync")
                {
                    System.Console.WriteLine("Is Employee Request InActive Users Start");
                    NotificationService notificationService = new NotificationService();
                    notificationService.EmployeeRequestInActiveUsers();
                    System.Console.WriteLine("Is Employee Request InActive Users End");
                }
                if (args[0].ToString() == "IsSaudaExpiredNotification")
                {
                    System.Console.WriteLine("Is Sauda Expired Notification Start");
                    NotificationService notificationService = new NotificationService();
                    notificationService.SaudaExpiredNotification();
                    System.Console.WriteLine("Is Sauda Expired Notification End");
                }

                if (args[0].ToString() == "IsOverDueNotification")
                {
                    System.Console.WriteLine("Is Over Due Notification Start");
                    NotificationService notificationService = new NotificationService();
                    notificationService.OverDueNotification();
                    System.Console.WriteLine("Is Over Due Notification End");
                }
                if (args[0].ToString() == "IsAuditLogDeleteSync")
                {
                    System.Console.WriteLine("Is Audit Log Delete Start");
                    AuditLogDeleteService customerLedgerService = new AuditLogDeleteService();
                    customerLedgerService.DeleteAuditLog();
                    System.Console.WriteLine("Is Audit Log Delete End");
                }
                if (args[0].ToString() == "IsPricingDeleteSync")
                {
                    System.Console.WriteLine("Is Pricing Delete Start");
                    PricingDeleteService customerLedgerService = new PricingDeleteService();
                    customerLedgerService.DeletePricing();
                    System.Console.WriteLine("Is Pricing Delete End");
                }
                if (args[0].ToString() == "IsLogFileMoveAndDeleteDeleteSync")
                {
                    System.Console.WriteLine("Is Log file move and delete Start");
                    LogFileDeleteService logFileDeleteService = new LogFileDeleteService();
                    logFileDeleteService.MoveAndDeleteLogFile();
                    System.Console.WriteLine("Is Log file move and delete End");
                }
                if (args[0].ToString() == "IsSapTemplateDeleteDeleteSync")
                {
                    System.Console.WriteLine("SAP Template delete Start");
                    SapTemplateDeleteService sapFileDeleteService = new SapTemplateDeleteService();
                    sapFileDeleteService.DeleteSAPTemplateFiles();
                    System.Console.WriteLine("SAP Template delete End");
                }
                if (args[0].ToString() == "IsBackupLogFileDeleteDeleteSync")
                {
                    System.Console.WriteLine("Backup Log file delete Start");
                    LogFileDeleteService logFileDeleteService = new LogFileDeleteService();
                    logFileDeleteService.DeleteBackupLogFile();
                    System.Console.WriteLine("Backup Log file delete End");
                }
                if (args[0].ToString() == "IsSaudaDuplicateDeleteSync")
                {
                    System.Console.WriteLine("Sauda Duplicate Delete Service Start");
                    SaudaDuplicateDeleteService saudaDuplicateDeleteService = new SaudaDuplicateDeleteService();
                    saudaDuplicateDeleteService.DeleteSaudaDuplicate();
                    System.Console.WriteLine("Sauda Duplicate Delete Service End");
                }
                if (args[0].ToString() == "IsLiftingSaudaOrderIdUpdate")
                {
                    System.Console.WriteLine("Lifting Sauda Order Id Update Service Start");
                    LiftingSaudaOrderUpdateService liftingupdate = new LiftingSaudaOrderUpdateService();
                    liftingupdate.LiftingSaudaOrderUpdate();
                    System.Console.WriteLine("Lifting Sauda Order Id Update Service End");
                }
                //var SapSaudaNumberMessageSync = Convert.ToBoolean(ConfigurationManager.AppSettings["IsUpdateSapSaudaNumberMessageSync"]);

                // if (SapSaudaNumberMessageSync)
                // {
                //     System.Console.WriteLine("Is UpdateSap SaudaNumber Message Sync Start");
                //     UpdateSapSaudaNumberMessage updateSapSaudaNumberMessageSync = new UpdateSapSaudaNumberMessage();
                //     updateSapSaudaNumberMessageSync.ProcessSaudaData();
                //     System.Console.WriteLine("Is UpdateSap SaudaNumber Message Sync End");
                // }

                if (args[0].ToString() == "IsResetDailyQuantityLimit")
                {
                    System.Console.WriteLine("Is Reset Daily Quantity Limit Start");
                    QuantityLimitService quantityLimitService = new QuantityLimitService();
                    quantityLimitService.ResetDailyQuantityLimit();
                    System.Console.WriteLine("Is Reset Daily Quantity Limit End");
                }
                if (args[0].ToString() == "IsPricingBackupAndCleanup")
                {
                    System.Console.WriteLine("Is Pricing Backup And Cleanup Start");
                    PricingBackupAndCleanupService pricingBackupAndCleanupService = new PricingBackupAndCleanupService();
                    pricingBackupAndCleanupService.BackupAndCleanupPricings();
                    System.Console.WriteLine("Is Pricing Backup And Cleanup End");
                }
            }
        }
    }
}
