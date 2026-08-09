using Adani.Solution.Data.DatabaseContext;
using GMCore.Helper;
using NLog;
using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Adani.Solution.Console.Common;

namespace Adani.Solution.Pricing.Services
{
    public class UpdateSapSaudaNumberMessage
    {
        private const string ServiceName = "UpdateSapSaudaNumberMessage";
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string connectionString = WebConfig.DBConnectionString;
        private readonly double _configMinutes;
        private readonly long _userId = WebConfig.SuperAdminUserId;

        public UpdateSapSaudaNumberMessage()
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string key = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes);
                    string configValue = conn.QueryFirstOrDefault<string>(
                        @"SELECT Value FROM Configurations WHERE [Key] = @Key",
                        new { Key = key });

                    if (!double.TryParse(configValue, out _configMinutes))
                    {
                        _configMinutes = 0;
                        _logger.Warn("Failed to parse configuration value for InboundInterfacenotSyncedToSAPMinutes.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"{ServiceName} - Constructor Exception: {ex}");
                _configMinutes = 0;
            }
        }

        public void ProcessSaudaData()
        {
            string methodName = "ProcessSaudaData";
            _logger.Info($"{ServiceName} - Method {methodName} started");

            try
            {
                DateTime currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                CallStoredProcedure(currentDate, _configMinutes, _userId);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ServiceName} - Method {methodName} Exception: {ex}");
            }
        }

        private void CallStoredProcedure(DateTime currentDate, double configMinutes, long userId)
        {
            _logger.Info("Calling stored procedure dbo.UpdateSapSaudaNumberMessage");

            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    conn.Execute("dbo.UpdateSapSaudaNumberMessage", new
                    {
                        CurrentDate = currentDate,
                        ConfigMinutes = (int)configMinutes,
                        UserId = userId
                    }, commandType: CommandType.StoredProcedure);

                    _logger.Info("Stored Procedure executed successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error while executing stored procedure: {ex.Message}");
            }
        }
    }
}