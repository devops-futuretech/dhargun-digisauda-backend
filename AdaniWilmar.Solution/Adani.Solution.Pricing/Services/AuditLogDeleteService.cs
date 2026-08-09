using Adani.Solution.Console.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Pricing.Services
{
    public class AuditLogDeleteService
    {
        private const string ServiceName = "AuditLogDeleteService";
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _methodName;

        static string connectionString = WebConfig.DBConnectionString;

        static long daysCount = WebConfig.AuditLogDeleteCount;

        public void DeleteAuditLog()
        {
            _methodName = "DeleteAuditLog";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "DeleteAuditLog";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
                    cmd.Parameters.AddWithValue("@DaysCount", daysCount);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    var rdr = cmd.ExecuteReader();
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
        }
    }
}
