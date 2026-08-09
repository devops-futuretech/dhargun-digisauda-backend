using Adani.Solution.Console.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Pricing.Services
{
    public class QuantityLimitService
    {
        private const string ServiceName = nameof(QuantityLimitService);
        private static GMCore.Logger.ILogger _logger = GMCore.Logger.Logging.GetLogger(nameof(QuantityLimitService));
        private string _methodName;
        static string connectionString = WebConfig.DBConnectionString;
        public void ResetDailyQuantityLimit()
        {
            _methodName = nameof(ResetDailyQuantityLimit);
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "[dbo].[ResetQuantityLimitsData]";
                    var cmd = new System.Data.SqlClient.SqlCommand(SP_Name, conn)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandTimeout = 0
                    };
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
        }
    }
}
