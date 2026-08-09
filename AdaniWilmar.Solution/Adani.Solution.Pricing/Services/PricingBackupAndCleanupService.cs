using Adani.Solution.Console.Common;
using NLog;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Adani.Solution.Pricing.Services
{
    public class PricingBackupAndCleanupService
    {
        private const string ServiceName = "PricingBackupAndCleanupService";
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _methodName;

        static string connectionString = WebConfig.DBConnectionString;

        public void BackupAndCleanupPricings()
        {
            _methodName = "BackupAndCleanupPricings";
            _logger.Info($"{ServiceName} Controller-Method {_methodName} - Started");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "BackupAndCleanupPricings";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    _logger.Info($"{ServiceName} Controller-Method {_methodName} - Completed Successfully");
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
                throw;
            }
        }
    }
}

