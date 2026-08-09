
using Adani.Solution.Console.Common;
using Dapper;
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
    public class VehicleTrackingUpdateService
    {
        private const string ServiceName = "SaudaDuplicateDeleteService";
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        private string _methodName;

        static string connectionString = WebConfig.DBConnectionString;
        public void GetVehicleTrackingData()
        {
            _methodName = "DeleteSaudaDuplicate";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {

                    var sqlQuery = @"create table #Temp(SaudaNumber varchar(max),count bigint,SaudaId bigint)
create table #TempSauda(SaudaId bigint,SaudaNumber varchar(max))
insert into #Temp(SaudaNumber,count,SaudaId)
select s.SaudaNumber,Count(*),Min(s.Id) from Saudas s 
where s.SaudaNumber is not null 
group by s.SaudaNumber
having count(*)>1
insert into #TempSauda(SaudaId,SaudaNumber)
select distinct so.SaudaId,s.SaudaNumber from SaudaOrders so
join Saudas s on s.Id=so.SaudaId
join #Temp t  on s.SaudaNumber=t.SaudaNumber
where s.Id!=t.SaudaId
delete from SaudaOrders where SaudaId in (select SaudaId from #TempSauda)
delete from Saudas where Id in (select SaudaId from #TempSauda)
drop table #Temp
drop table #TempSauda";
                    var saudaListDto = conn.Execute(sqlQuery, new
                    {
                    });

                }

            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
        }
    }
}
