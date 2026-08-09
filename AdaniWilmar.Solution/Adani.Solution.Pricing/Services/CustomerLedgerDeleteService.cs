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
    public class CustomerLedgerDeleteService
    {
        private const string ServiceName = "CustomerLedgerDeleteService";
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _methodName;

        static string connectionString = WebConfig.DBConnectionString;

        public void DeleteCustomerLedger()
        {
            _methodName = "DeleteCustomerLedger";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "DeleteCustomerLedger";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
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
