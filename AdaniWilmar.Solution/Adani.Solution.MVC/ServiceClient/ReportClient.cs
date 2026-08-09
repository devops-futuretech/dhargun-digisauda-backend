using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Adani.Solution.MVC.Helpers;
using System.Configuration;
using System.Data.SqlClient;
using Adani.Solution.DTO.Enums;
using System.Reflection;
using Adani.Solution.DTO.Common;
using Adani.Solution.MVC.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Adani.Solution.MVC.ServiceClient
{
    public class ReportClient : BaseClient
    {
        private const string ServiceName = "Report Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;
        static string connectionString = ConfigHelper.SPConnectionString;

        #region Get Report

        public async Task<DataTable> GetReport(string apiUrl, object inputDto)
        {
            var result = new DataTable();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(apiUrl, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<DataTable>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region OilPrice Export

        public async Task<DataTable> GetOilPriceReportAsync(OilPriceReportInputDto inputDto)
        {
            _methodName = "GetOilPriceReportAsync";
            return await GetReport(ApiUrl.WebApiUrlOilPriceReport, inputDto);

        }

        #endregion

        #region CostChange Export

        public async Task<DataTable> GetCostChangeReport(ReportInputDto inputDto)
        {
            _methodName = "GetCostChangeReport";
            return await GetReport(ApiUrl.WebApiUrlCostChangeReport, inputDto);
        }

        #endregion

        #region Sauda Export

        public async Task<List<SaudaOrderReportOutputDto>> GetSaudaOrderDetailsReportAsync(SaudaOrderReportInputputDto inputDto)
        {
            _methodName = "GetSaudaOrderDetailsReport";
            var result = await GetListAsync<SaudaOrderReportOutputDto>(ApiUrl.WebApiUrlSaudaOrdersReport, inputDto);
            return result.ToList();
        }

        public async Task<List<DistributorStockReportOutputDto>> GetDistributorStockReportAsync(DistributorStockReportInputDto inputDto)
        {
            _methodName = "GetDistributorStockReport";
            var result = await GetListAsync<DistributorStockReportOutputDto>(ApiUrl.WebApiUrlDistributorStockReport, inputDto);
            return result.ToList();
        }
        public async Task<List<ActualSaudaOrderReportOutputDto>> GetNewSaudaOrderDetailsReportAsync(SaudaOrderReportInputputDto inputDto)
        {
            _methodName = "GetSaudaOrderDetailsReport";
            var result = new List<ActualSaudaOrderReportOutputDto>();
            var stateIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateIds);
            var status = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StatusIds);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        await connection.OpenAsync();
                        result = connection.QueryAsync<ActualSaudaOrderReportOutputDto>("SaudaOrderExport", new
                        {
                            @RoleId = inputDto.RoleId,
                            @LoginUserId = inputDto.LoginUserId,
                            @FromDate = inputDto.FromDate,
                            @ToDate = inputDto.ToDate,
                            @StateIds = stateIds,
                            @StatusIds = status,
                            @VerticalId = inputDto.VerticalId,
                            @SalesOrganizationId = inputDto.SalesOrganizationId,
                            @DistributionChannelId = inputDto.DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 0).Result.ToList();

                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                return result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return result;
            }
        }

        public async Task<DataSet> GetUserLoginHistoryReport(UserLoginHistoryDto inputDto)
        {
            _methodName = "GetUserLoginHistoryReport";
            var result = new DataSet();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        //connection.Open();
                        //var data = connection.QueryMultiple("UserLoginHistoryExport", new
                        //{
                        //    @FromDate = inputDto.FromDate,
                        //    @ToDate = inputDto.ToDate,
                        //    @LoginUserId = inputDto.LoginUserId,
                        //}, commandType: System.Data.CommandType.StoredProcedure);

                        connection.Open();
                        SqlCommand cmd = new SqlCommand("UserLoginHistoryExport", connection);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandTimeout = 120;
                        cmd.Parameters.AddWithValue("@FromDate", inputDto.FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", inputDto.ToDate);
                        cmd.Parameters.AddWithValue("@LoginUserId", inputDto.LoginUserId);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(result);

                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }



        public async Task<List<SaudaLimitDto>> SaudaLimitExportAsync(ReportFilterDto inputDto)
        {
            _methodName = "SaudaLimitExportAsync";
            var result = new List<SaudaLimitDto>();

            var stateIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateIds);
            var zonalHead = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.zhId);
            var bdoIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.bdoId);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<SaudaLimitDto>("ContractLimitReport", new
                        {
                            @LoginUserId = inputDto.LoginUserId,
                            @RoleId = inputDto.RoleId,
                            @ZhIds = zonalHead,
                            @BdoIds = bdoIds,
                            @DealerCode = inputDto.dealerCode,
                            @StateIds = stateIds,
                            @VerticalId = inputDto.DivisionId,
                            @SalesOrganizationId = inputDto.SalesOrganizationId,
                            @DistributionChannelId = inputDto.DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            //return result;

            //var result1 = await GetListAsync<SaudaLimitDto>(ApiUrl.WebApiUrlSaudaLimitExport, inputDto);
            return result.ToList();
        }

        public async Task<DataSourceResult> CreditLimitExportAsync(ReportFilterDto inputDto)
        {
            _methodName = "CreditLimitExportAsync";
            var result = new List<HANACreditMasterDto>();



            var stateIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateIds);
            var zonalHead = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.zhId);
            var bdoIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.bdoId);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<HANACreditMasterDto>("CreditLimitExport", new
                        {
                            @LoginUserId = inputDto.LoginUserId,
                            @RoleId = inputDto.RoleId,
                            @ZhIds = zonalHead,
                            @BdoIds = bdoIds,
                            @DealerCode = inputDto.dealerCode,
                            @StateIds = stateIds,
                            @VerticalId = inputDto.VerticalId,
                            @SalesOrganizationId = inputDto.SalesOrganizationId,
                            @DistributionChannelId = inputDto.DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            var datasourceResult = result.ToDataSourceResult(inputDto.DataSourceRequest);
            //var result = await GetKendoGridResultAsync<HANACreditMasterDto>(ApiUrl.WebApiUrlCreditLimitExport, inputdto);
            return datasourceResult;
        }

        public async Task<List<HANACreditMasterDto>> GetCreditLimitAsync(ReportFilterDto inputDto)
        {
            _methodName = "CreditLimitExportAsync";

            var result = new List<HANACreditMasterDto>();



            var stateIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateIds);
            var zonalHead = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.zhId);
            var bdoIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.bdoId);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<HANACreditMasterDto>("CreditLimitExport", new
                        {
                            @LoginUserId = inputDto.LoginUserId,
                            inputDto.RoleId,
                            @ZhIds = zonalHead,
                            @BdoIds = bdoIds,
                            @DealerCode = inputDto.dealerCode,
                            @StateIds = stateIds,
                            @VerticalId = inputDto.VerticalId,
                            @SalesOrganizationId = inputDto.SalesOrganizationId,
                            @DistributionChannelId = inputDto.DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            //return result;


            //var result1 = (List<HANACreditMasterDto>)await GetListAsync<HANACreditMasterDto>(ApiUrl.WebApiUrlCreditLimitExport, inputDto);
            return result.ToList();
        }

        public async Task<List<SalesBDOWiseReportDto>> SalesExportAsync(SalesReportInputDto inputDto)
        {
            _methodName = "SalesExportAsync";
            var result = await GetListAsync<SalesBDOWiseReportDto>(ApiUrl.WebApiUrlSalesExport, inputDto);
            return result.ToList();
        }

        public async Task<PendingContractReportDto> GetVerticalIdAsync(long UserId)
        {
            _methodName = "GetVerticalIdAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var apiUrl = ApiUrl.WebApiUrlGetVerticalId;
            return await GetById<PendingContractReportDto>(apiUrl, UserId);
        }

        public List<PendingContractReportOutputDto> PendingContractExportAsync(PendingContractReportDto inputDto)
        {
            _methodName = "PendingContractExportAsync";
            //var result = await GetListAsync<PendingContractReportOutputDto>(ApiUrl.WebApiUrlPendingContractExport, inputDto);

            using (IDbConnection conn = new SqlConnection(connectionString))
            {
                var result = conn.Query<PendingContractReportOutputDto>("SP_Emami_PendingContractReport", new
                {
                    OilTypeIds = string.Join(",", inputDto.OilTypeIds != null ? inputDto.OilTypeIds : new List<long>()),
                    BDOIds = string.Join(",", inputDto.BDOIds != null ? inputDto.BDOIds : new List<long>()),
                    LoginUserId = inputDto.LoginUserId
                }, commandType: System.Data.CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<SaudaBDOWiseReportDto>> SaudaExportAsync(SaudaOrderReportInputputDto inputDto)
        {
            _methodName = "SaudaExportAsync";
            var result = await GetListAsync<SaudaBDOWiseReportDto>(ApiUrl.WebApiUrlSaudaExport, inputDto);
            return result.ToList();
        }

        #endregion

        #region SaudaAgingReport

        public List<SaudaAgingReportExportDto> SaudaAgingReport(SaudaAgingReportDto inputDto)
        {
            _methodName = "SaudaAgingReport";

            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SaudaAgingReportExportDto> result = new List<SaudaAgingReportExportDto>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        result = connection.Query<SaudaAgingReportExportDto>("SP_SaudaAgingReport", new
                        {
                            inputDto.Party,
                            inputDto.PartyName,
                            inputDto.CityId,
                            inputDto.MaterialDescription,
                            inputDto.DepotId,
                            inputDto.SaudaAging

                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;

        }


        #endregion

        #region Margin Export

        public List<ReportPlantwiseSaudaOuputDto> GetMarginReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "ReportPlantwiseSauda";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<ReportPlantwiseSaudaOuputDto> result = new List<ReportPlantwiseSaudaOuputDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        if (inputDto.MarginTypeId == (int)DTO.Enums.MarginReport.PlantwiseSauda)
                        {
                            result = connection.Query<ReportPlantwiseSaudaOuputDto>("Sp_Emami_ReportPlantwiseSauda", new
                            {
                                inputDto.FromDate,
                                inputDto.ToDate,
                                inputDto.StatusIds,
                                inputDto.StateIds,
                                inputDto.VerticalId
                            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                        }
                        else if (inputDto.MarginTypeId == (int)DTO.Enums.MarginReport.StateOilMargin)
                        {
                            result = connection.Query<ReportPlantwiseSaudaOuputDto>("Sp_Emami_ReportStateAndOilMargin", new
                            {
                                inputDto.FromDate,
                                inputDto.ToDate,
                                inputDto.StatusIds,
                                inputDto.StateIds,
                                inputDto.VerticalId
                            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                        }
                        else
                        {
                            result = connection.Query<ReportPlantwiseSaudaOuputDto>("Sp_Emami_ReportBusinessMargin", new
                            {
                                inputDto.FromDate,
                                inputDto.ToDate,
                                inputDto.StatusIds,
                                inputDto.StateIds,
                                inputDto.VerticalId
                            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                        }
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        public IList<DropDownDto> GetMarginReportType()
        {
            _methodName = "GetMarginReportType";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");

            var resultList = new List<DropDownDto>();
            foreach (var item in Settings.EnumToList<MarginReport>())
            {
                var aleTypeItem = new DropDownDto
                {
                    Name = Settings.GetEnumDescription(item),
                    Id = (int)item
                };
                resultList.Add(aleTypeItem);
            }
            return resultList;
        }

        #endregion

        #region DepotCost Export

        public List<DepotCostReportDto> GetDepotCostReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "GetDepotCostReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DepotCostReportDto> result = new List<DepotCostReportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<DepotCostReportDto>("SP_Emami_DepotCostReport", new
                        {
                            inputDto.FromDate,
                            inputDto.ToDate,
                            inputDto.VerticalId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion
        #region Stock Report by Plant
        public List<SalesExportDto> GetStockReport(SaleReportDto inputDto)
        {
            _methodName = "GetStockReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SalesExportDto> result = new List<SalesExportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<SalesExportDto>("SP_StockReport", new
                        {
                            inputDto.PlantId,
                            inputDto.Name,
                            inputDto.MaterialId,
                            inputDto.MaterialDescription,
                            inputDto.Message,
                            //inputDto.FromDate,
                            //inputDto.ToDate,
                            //inputDto.VerticalId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }
        #endregion
        #region DetentionCost Export

        public List<DepotCostReportDto> GetDetentionCostReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "GetDetentionCostReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DepotCostReportDto> result = new List<DepotCostReportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<DepotCostReportDto>("SP_Emami_DetentionCostReport", new
                        {
                            inputDto.FromDate,
                            inputDto.ToDate,
                            inputDto.VerticalId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }
        public List<TargetVsAchievementExportDto> GetTargetVsAchievementReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "GetDepotCostReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<TargetVsAchievementExportDto> res = new List<TargetVsAchievementExportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        var result = connection.Query<TargetVsAchievementReportDto>("SP_Report_TargetVsAcheivement", new
                        {
                            @StartDate = inputDto.FromDate,
                            @EndDate = inputDto.ToDate,
                            @VerticalId = inputDto.VerticalId,
                            inputDto.SalesOrganizationId,
                            inputDto.DistributionChannelId,
                            inputDto.LoginUserId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                        res = result.Select(s => new TargetVsAchievementExportDto()
                        {
                            Achievement = s.Achievement,
                            AchievementPercentage = s.AchievementPercentage,
                            BDOKAM = s.BDOKAM,
                            StateName = s.StateName,
                            Target = s.Target,
                            ZonalTrader = s.ZonalTrader
                        }).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return res;
        }
        public List<MTPVsDSRDeviationReportDto> GetMTPVsDSRDeviationReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "GetPCPVsMTPDeviationReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<MTPVsDSRDeviationReportDto> result = new List<MTPVsDSRDeviationReportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<MTPVsDSRDeviationReportDto>("SP_MTPVsDSRDeviationReport", new
                        {
                            @StartDate = inputDto.FromDate,
                            @EndDate = inputDto.ToDate,
                            @StateIds = inputDto.StateIds,
                            @BDOIds = inputDto.BDOIds,
                            @VerticalId = inputDto.VerticalId,
                            inputDto.SalesOrganizationId,
                            inputDto.DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }
        public List<MTPVsDSRDeviationReportDto> GetPCPVsMTPDeviationReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "GetPCPVsMTPDeviationReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<MTPVsDSRDeviationReportDto> result = new List<MTPVsDSRDeviationReportDto>();

            DataSet dsresult = new DataSet();
            DataTable dtFinalResult = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        result = connection.Query<MTPVsDSRDeviationReportDto>("SP_KEINTO_PCPVsMTPDeviationReport", new
                        {
                            @StartDate = inputDto.FromDate,
                            @EndDate = inputDto.ToDate,
                            @StateIds = inputDto.StateIds,
                            @BDOIds = inputDto.BDOIds,
                            @VerticalId = inputDto.VerticalId,
                            inputDto.SalesOrganizationId,
                            inputDto.DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }
        public DataTable GetCompetitorRateReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "GetCompetitorRateReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<MTPVsDSRDeviationReportDto> result = new List<MTPVsDSRDeviationReportDto>();

            DataSet dsresult = new DataSet();
            DataTable dtFinalResult = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("SP_KEINTO_CompetitorRateReport", connection);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", inputDto.FromDate);
                        cmd.Parameters.AddWithValue("@EndDate", inputDto.ToDate);
                        cmd.Parameters.AddWithValue("@VerticalId", inputDto.VerticalId);
                        cmd.Parameters.AddWithValue("@SalesOrganizationId", inputDto.SalesOrganizationId);
                        cmd.Parameters.AddWithValue("@DistributionChannelId", inputDto.DistributionChannelId);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dsresult);
                        dtFinalResult = FormCompetetorRateReport(dsresult);
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return dtFinalResult;
        }
        public DataTable FormCompetetorRateReport(DataSet dataSet)
        {
            DataTable dtCompetitors = new DataTable();
            DataTable dtColumns = new DataTable();
            DataTable dtResult = new DataTable();

            dtCompetitors = dataSet.Tables[0];
            dtColumns = dataSet.Tables[1];
            dtResult = dataSet.Tables[2];

            DataTable dtFinalResult = new DataTable("CompetitorRateReport");
            DataColumn dtColumn;
            DataRow myDataRow;

            foreach (DataRow row in dtColumns.Rows)
            {
                dtColumn = new DataColumn();
                dtColumn.DataType = typeof(string);
                dtColumn.ColumnName = row["ColumnName"].ToString();
                dtColumn.Caption = row["ColumnName"].ToString();
                dtFinalResult.Columns.Add(dtColumn);
            }

            foreach (DataRow row in dtResult.Rows)
            {
                bool exists = dtFinalResult.Select().ToList().Exists(_ => _["StateId"].ToString().ToUpper() == row["StateId"].ToString() && _["ProductId"].ToString().ToUpper() == row["SkuId"].ToString() && _["Name Of Market"].ToString().ToUpper() == row["NameOfMarket"].ToString().ToUpper());
                if (!exists)
                {
                    myDataRow = dtFinalResult.NewRow();
                    myDataRow["StateId"] = row["StateId"].ToString();
                    myDataRow["State"] = row["StateName"].ToString();
                    myDataRow["ProductId"] = row["SkuId"].ToString();
                    myDataRow["Product"] = row["Product"].ToString();
                    myDataRow["Name Of Market"] = row["NameOfMarket"].ToString();
                    dtFinalResult.Rows.Add(myDataRow);
                }
            }
            foreach (DataRow row in dtCompetitors.Rows)
            {
                var competitorResult = from competitor in dtResult.AsEnumerable()
                                       where competitor.Field<string>("Competitor") == row["Competitor"].ToString()
                                       select competitor;

                foreach (DataRow item in competitorResult.ToList())
                {
                    foreach (DataRow finalresultrow in dtFinalResult.Rows)
                    {
                        if (finalresultrow["StateId"].ToString() == item["StateId"].ToString() && finalresultrow["ProductId"].ToString() == item["SkuId"].ToString() && finalresultrow["Name Of Market"].ToString() == item["NameOfMarket"].ToString())
                        {
                            if (finalresultrow[row["Competitor"].ToString() + " - MOP"].ToString() != "")
                            {
                                finalresultrow[row["Competitor"].ToString() + " - MOP"] = Convert.ToDecimal(finalresultrow[row["Competitor"].ToString() + " - MOP"]) + Convert.ToDecimal(item["MOP"]);
                                finalresultrow[row["Competitor"].ToString() + " - PTD"] = Convert.ToDecimal(finalresultrow[row["Competitor"].ToString() + " - PTD"]) + Convert.ToDecimal(item["PTD"]);
                            }
                            else
                            {
                                finalresultrow[row["Competitor"].ToString() + " - MOP"] = item["MOP"];
                                finalresultrow[row["Competitor"].ToString() + " - PTD"] = Convert.ToDecimal(item["PTD"]);
                            }
                        }
                    }
                }
            }
            return dtFinalResult;
        }
        public DataTable GetDailyStatusReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "GetDailyStatusReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DataTable result = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("Sp_DailyStatusReport", connection);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(result);
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        public List<PriceReleaseAuditReportDto> GetPriceReleaseAuditReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "GetPriceReleaseAuditReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<PriceReleaseAuditReportDto> result = new List<PriceReleaseAuditReportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PriceReleaseAuditReportDto>("Sp_Report_PriceReleaseAudit", new
                        {
                            @StartDate = inputDto.FromDate,
                            @EndDate = inputDto.ToDate,
                            @VerticalId = inputDto.VerticalId,
                            @PlantId = inputDto.PlantId,
                            inputDto.SalesOrganizationId,
                            inputDto.DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        public List<SaudaExecutionReportDto> GetSaudaExecutionAuditReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "GetSaudaExecutionAuditReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SaudaExecutionReportDto> result = new List<SaudaExecutionReportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<SaudaExecutionReportDto>("Sp_Report_SaudaExecutionAudit", new
                        {
                            @StartDate = inputDto.FromDate,
                            @EndDate = inputDto.ToDate,
                            @VerticalId = inputDto.VerticalId,
                            @PlantId = inputDto.PlantId,
                            inputDto.SalesOrganizationId,
                            inputDto.DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }
        #endregion

        #region Indent/Lifting Export

        public List<LiftingListReportDto> IndentReportExport(IndentReportInputDto inputDto)
        {
            _methodName = "IndentReportExport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<LiftingListReportDto> result = new List<LiftingListReportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {

                        var StateIds = inputDto.StateIds != null ? String.Join(",", inputDto.StateIds) : "";
                        connection.Open();
                        result = connection.Query<LiftingListReportDto>("Sp_Emami_IndentReport", new
                        {
                            inputDto.StatusId,
                            inputDto.StartDate,
                            inputDto.EndDate,
                            StateIds,
                            // inputDto.IsAfterDeliverOrderNumber,
                            inputDto.verticalIds,
                            inputDto.LoginUserId,
                            inputDto.RoleId,
                            inputDto.SalesOrganizationId,
                            inputDto.DistributionChannelId

                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region MTP

        public async Task<List<MonthlyTourPlanOutputDto>> GetMTPDetailsReportAsync(MonthlyTourPlanReportInputDto inputDto)
        {
            _methodName = "GetMTPDetailsReportAsync";

            var result = new List<MonthlyTourPlanOutputDto>();
            try
            {
                var zonalHead = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.ZonalHeadIds);
                var bdoIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.BDOIds);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<MonthlyTourPlanOutputDto>("MTPReport", new
                        {
                            ZhIds = zonalHead,
                            BdoIds = bdoIds,
                            VerticalId = inputDto.VerticalId,
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            DistributionChannelId = inputDto.DistributionChannelId,
                            FromDate = inputDto.FromDate,
                            ToDate = inputDto.ToDate
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                        //result = result.Where(_ => _.CreatedDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList(); ;
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;

            //var result = await GetListAsync<MonthlyTourPlanOutputDto>(ApiUrl.WebApiUrlMTPReport, inputDto);
            //return result.ToList();
        }
        #endregion

        #region PCP
        public async Task<List<PermanentCoveragePlanReportOutputDto>> GetPCPDetailsReportAsync(PermanentCoveragePlanReportInputDto inputDto)
        {
            _methodName = "GetMTPDetailsReportAsync";
            var result = new List<PermanentCoveragePlanReportOutputDto>();

            try
            {
                var zonalHead = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.ZonalHeadIds);
                var bdoIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.BDOIds);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PermanentCoveragePlanReportOutputDto>("PCPReport", new
                        {
                            ZhIds = zonalHead,
                            BdoIds = bdoIds,
                            VerticalId = inputDto.VerticalId,
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            DistributionChannelId = inputDto.DistributionChannelId,
                            FromDate = inputDto.FromDate,
                            ToDate = inputDto.ToDate
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                        //result = result.Where(_ => _.CreatedDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList(); ;
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;


            //var result = await GetListAsync<PermanentCoveragePlanReportOutputDto>(ApiUrl.WebApiUrlPCPReport, inputDto);
            //return result.ToList();
        }
        #endregion

        #region Monthly Report
        public DataTable MonthWiseInvoiceExportToList(MonthlyReportInputDto InputDto)
        {
            _methodName = "MonthWiseInvoiceExportToList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "GetInvoiceDataExport";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ValidFrom", InputDto.StartDate);
                    cmd.Parameters.AddWithValue("@ValidTo", InputDto.EndDate);
                    cmd.Parameters.AddWithValue("@VerticalId", InputDto.VerticalId);
                    cmd.Parameters.AddWithValue("@SalesOrganizationId", InputDto.SalesOrganizationId);
                    cmd.Parameters.AddWithValue("@DistributionChannelId", InputDto.DistributionChannelId);
                    cmd.Parameters.AddWithValue("@LoginUserId", InputDto.LoginUserId);
                    cmd.Parameters.AddWithValue("@RoleId", InputDto.RoleId);
                    cmd.CommandTimeout = 0;
                    var rdr = cmd.ExecuteReader();
                    dt.Load(rdr);
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return dt;
        }

        public DataTable MonthWiseSaudaExportToList(MonthlyReportInputDto InputDto)
        {
            _methodName = "MonthWiseSaudaExportToList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "GetSaudaDataExport";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ValidFrom", InputDto.StartDate);
                    cmd.Parameters.AddWithValue("@ValidTo", InputDto.EndDate);
                    cmd.Parameters.AddWithValue("@VerticalId", InputDto.VerticalId);
                    cmd.Parameters.AddWithValue("@SalesOrganizationId", InputDto.SalesOrganizationId);
                    cmd.Parameters.AddWithValue("@DistributionChannelId", InputDto.DistributionChannelId);
                    cmd.Parameters.AddWithValue("@LoginUserId", InputDto.LoginUserId);
                    cmd.Parameters.AddWithValue("@RoleId", InputDto.RoleId);
                    cmd.CommandTimeout = 0;
                    var rdr = cmd.ExecuteReader();
                    dt.Load(rdr);
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return dt;
        }

        public DataTable MonthWiseLiftingRequestExportToList(MonthlyReportInputDto InputDto)
        {
            _methodName = "MonthWiseLiftingRequestExportToList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "GetLiftingRequestDataExport";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ValidFrom", InputDto.StartDate);
                    cmd.Parameters.AddWithValue("@ValidTo", InputDto.EndDate);
                    cmd.Parameters.AddWithValue("@VerticalId", InputDto.VerticalId);
                    cmd.Parameters.AddWithValue("@SalesOrganizationId", InputDto.SalesOrganizationId);
                    cmd.Parameters.AddWithValue("@DistributionChannelId", InputDto.DistributionChannelId);
                    cmd.Parameters.AddWithValue("@LoginUserId", InputDto.LoginUserId);
                    cmd.Parameters.AddWithValue("@RoleId", InputDto.RoleId);
                    cmd.CommandTimeout = 0;
                    var rdr = cmd.ExecuteReader();
                    dt.Load(rdr);
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Controller-Method {_methodName} Exception: {exception}");
            }
            return dt;
        }
        #endregion


        #region DSRReport

        public List<DSRReportDTO> DSRReportExport(DSRReportInputdto dSRReportInputdto)
        {
            _methodName = "IndentReportExport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<DSRReportDTO> result = new List<DSRReportDTO>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        var BDOIds = dSRReportInputdto.BDOIds != null && !dSRReportInputdto.BDOIds.Contains(0) ? String.Join(",", dSRReportInputdto.BDOIds) : "";
                        var ZHIds = dSRReportInputdto.ZHIds != null && !dSRReportInputdto.ZHIds.Contains(0) ? String.Join(",", dSRReportInputdto.ZHIds) : "";
                        if (dSRReportInputdto.ReportType == (int)DTO.Enums.DSRReportType.DealerVisit)
                        {

                            connection.Open();
                            result = connection.Query<DSRReportDTO>("SP_Emami_DSRReport", new
                            {
                                dSRReportInputdto.FromDate,
                                dSRReportInputdto.ToDate,
                                BDOIds,
                                ZHIds,
                                dSRReportInputdto.VerticalId,
                                dSRReportInputdto.SalesOrganizationId,
                                dSRReportInputdto.DistributionChannelId
                            }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                        }
                        if (dSRReportInputdto.ReportType == (int)DTO.Enums.DSRReportType.Wholesaler)
                        {

                            connection.Open();
                            result = connection.Query<DSRReportDTO>("SP_Emami_WholeSellerVisitReport", new
                            {
                                dSRReportInputdto.FromDate,
                                dSRReportInputdto.ToDate,
                                BDOIds,
                                ZHIds,
                                dSRReportInputdto.VerticalId,
                                dSRReportInputdto.SalesOrganizationId,
                                dSRReportInputdto.DistributionChannelId
                            }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                        }
                        if (dSRReportInputdto.ReportType == (int)DTO.Enums.DSRReportType.ProspectiveDealer)
                        {

                            connection.Open();
                            result = connection.Query<DSRReportDTO>("SP_Emami_ProspectiveDealerReport", new
                            {
                                dSRReportInputdto.FromDate,
                                dSRReportInputdto.ToDate,
                                BDOIds,
                                ZHIds
                            }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                        }
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }
        #endregion


        #region PendingContracts

        public List<PendingContractstDto> GetPendingContractsList(long loginUserId, long roleId, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetPendingContractsList";
            var result = new List<PendingContractstDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PendingContractstDto>("GetPendingContracts", new
                        {
                            LoginUserId = loginUserId,
                            RoleId = roleId,
                            DivisionId = verticalId,
                            SalesOrgId = SalesOrganizationId,
                            DistChannelId = DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                        //result = result.Where(_ => _.CreatedDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList(); ;
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }


        #endregion


        #region PendingContractTrigger

        public async Task<ResultDto> GetPendingContractTrigger(ContractOBRInputDto input, List<OpenContractRequestDTO> dealerCodes)
        {
            _methodName = "GetPendingContractsList";
            var result = new ResultDto();
            try
            {
                _methodName = "GetDealerListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var SalesOrgCode = "";
                var DistCode = "";
                var DivisionCode = "";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string salesCode = "Select Code from SalesOrganizations where Id=@Id";
                        string dist = "Select Code from DistributionChannels where Id=@Id";
                        string division = "Select Code from Divisions where Id=@Id";
                        SalesOrgCode = connection.Query<string>(salesCode, new
                        {
                            Id = input.SalesOrgId
                        }).FirstOrDefault();
                        DistCode = connection.Query<string>(dist, new
                        {
                            Id = input.DistChnlId
                        }).FirstOrDefault();
                        DivisionCode = connection.Query<string>(division, new
                        {
                            Id = input.DivisionId
                        }).FirstOrDefault();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                var inputDto = new OpenContractRequestDTOList()
                {
                    Records = dealerCodes,
                    SalesOrg = SalesOrgCode,
                    DistChannel = DistCode,
                    Division = DivisionCode
                };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<OpenContractRequestDTOList>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPendingContractTrigger, inputSring);

                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));

                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<ResultDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion


        #region PendingContractComparision

        public List<PendingContractComparisionOutputDto> GetPendingContractComparisionList(long VerticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetPendingContractsList";
            var result = new List<PendingContractComparisionOutputDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<PendingContractComparisionOutputDto>("GetPendingContractComparisionList", new
                        {
                            VerticalId,
                            SalesOrganizationId,
                            DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                        foreach (var data in result)
                        {
                            var status = "";
                            var actiontobetaken = "";

                            if (data.SAPDealerCode != data.DealerCode)
                            {
                                status = "Error";
                                actiontobetaken = "Dealer Code";
                            }
                            if (data.SAPContractNumber != data.ContractNumber)
                            {
                                status = "Error";
                                actiontobetaken = actiontobetaken + ",Contract Number";
                            }
                            if (data.SAPContractQuantity != data.ContractQuantity)
                            {
                                status = "Error";
                                actiontobetaken = actiontobetaken + ",Contract Quantity";
                            }
                            if (data.SAPDespatchQuantity != data.DespatchQuantity)
                            {
                                status = "Error";
                                actiontobetaken = actiontobetaken + ",Despatch Quantity";
                            }
                            if (data.SAPPendingQuantity != data.PendingQuantity)
                            {
                                status = "Error";
                                actiontobetaken = actiontobetaken + ",Pending Quantity";
                            }
                            if (data.SAPMaterialCode != data.MaterialCode)
                            {
                                status = "Error";
                                actiontobetaken = actiontobetaken + ",Material Code";
                            }
                            if (status == "")
                            {
                                status = "ok";
                                actiontobetaken = "No action to be taken";
                            }
                            else
                            {
                                actiontobetaken = actiontobetaken.TrimStart(',') + " Mismatched.";
                            }
                            data.Status = status;
                            data.ActionToTaken = actiontobetaken;
                        }

                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion


        #region SalesRegister
        public List<SalesRegisterOutputDto> GetSalesRegisterList(long loginUserId, long roleId, DateTime monthStartdate, DateTime monthLastdate, long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetSalesRegisterList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SalesRegisterOutputDto> result = new List<SalesRegisterOutputDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<SalesRegisterOutputDto>("GetSalesRegisterList", new
                        {
                            MonthStartdate = monthStartdate.ToString("yyyy-MM-dd"),
                            MonthLastdate = monthLastdate.ToString("yyyy-MM-dd"),
                            LoginUserId = loginUserId,
                            RoleId = roleId,
                            VerticalId = verticalId,
                            SalesOrganizationId,
                            DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                        //result=result.Where(_ => _.CreatedDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion


        #region SalesRegisterComparison
        public List<SalesRegisterOutputDto> GetSalesRegisterComparisonList(long VerticalId, DateTime StartDate, DateTime EndDate, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetSalesRegisterComparisonList";
            var result = new List<SalesRegisterOutputDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<SalesRegisterOutputDto>("GetSalesRegisterComparisionList", new
                        {
                            VerticalId,
                            StartDate,
                            EndDate,
                            SalesOrganizationId,
                            DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                        foreach (var data in result)
                        {
                            var status = "";
                            var actiontobetaken = "";

                            if (data.BillNumber != data.InvBillNumber)
                            {
                                status = "Error";
                                actiontobetaken = "Bill Number ";
                            }
                            if (data.QuantityCase != data.InvQuantityInCase)
                            {
                                status = "Error";
                                actiontobetaken = actiontobetaken + ",Quantity In Case";
                            }
                            if (status == "")
                            {
                                status = "ok";
                                actiontobetaken = "No action to be taken";
                            }
                            else
                            {
                                actiontobetaken = actiontobetaken.TrimStart(',') + " Mismatched.";
                            }
                            data.Status = status;
                            data.ActionToTaken = actiontobetaken;
                        }

                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region RA Sauda Report

        public List<RaSaudaOrederReportDto> GetRaSaudaOrderReport(DateTime FromDate, DateTime ToDate, List<long> stateIds, int verticalId, List<long> statusIds)
        {
            var saudaOrders = new List<RaSaudaOrederReportDto>();
            try
            {
                #region Vertical
                string VerticalId = string.Empty;
                List<long> verticals = new List<long>();
                if (verticalId == 0)
                {
                    verticals.Add((int)DTO.Enums.Division.Hbc);
                    verticals.Add((int)DTO.Enums.Division.SpecialityFat);
                }
                else
                    verticals.Add(verticalId);

                VerticalId = string.Join(",", verticals);
                #endregion

                string StatusIds = string.Join(",", statusIds);

                string StateIds = "";
                if (stateIds.IsAny())
                {
                    StateIds = string.Join(",", stateIds);
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    saudaOrders = connection.Query<RaSaudaOrederReportDto>("SP_GetRaSaudaOrderReport",
                    new
                    {
                        VerticalId,
                        StatusIds,
                        StateIds,
                        FromDate,
                        ToDate
                    }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return saudaOrders;
        }

        #endregion

        #region Sauda Conversion Report

        public async Task<List<SaudaConversionDetailsBySkuId>> GetSaudaConversionReport(SaudaConversionReportInputDto inputDto)
        {
            _methodName = "GetSaudaConversionReport";
            var result = new List<SaudaConversionDetailsBySkuId>();
            var responseoutput = new SaudaConversionSKUStatusListModel();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlSaudaConversionReport, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<SaudaConversionDetailsBySkuId>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        responseoutput.PostStatus = false;
                        responseoutput.PostStatusMessage = errorDtoResult.Message;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                responseoutput.PostStatus = false;
                responseoutput.PostStatusMessage = exception.Message;
            }
            return result;
        }

        #endregion

        #region New Sauda Report 

        public List<NewSaudaReportOutputDto> GetNewSaudaReport(long VerticalId, DateTime FromDate, DateTime ToDate, long LoginUserId, long RoleId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetNewSaudaReport";
            var result = new List<NewSaudaReportOutputDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<NewSaudaReportOutputDto>("GetNewSaudaReport", new
                        {
                            VerticalId,
                            FromDate,
                            ToDate,
                            LoginUserId,
                            RoleId,
                            SalesOrganizationId,
                            DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }
        #endregion

        #region Sauda Modification Report

        public async Task<List<SaudaModificationReportOutputDto>> GetSaudaModificationReportAsync(SaudaOrderReportInputputDto inputDto)
        {
            _methodName = "GetSaudaModificationReport";
            var result = new List<SaudaModificationReportOutputDto>();
            var stateIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateIds);
            var statusIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StatusIds);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        await connection.OpenAsync();
                        result = connection.QueryAsync<SaudaModificationReportOutputDto>("GetSaudaModificationReport", new
                        {
                            @RoleId = inputDto.RoleId,
                            @LoginUserId = inputDto.LoginUserId,
                            @FromDate = inputDto.FromDate,
                            @ToDate = inputDto.ToDate,
                            @StateIds = stateIds,
                            @StatusIds = statusIds,
                            @VerticalId = inputDto.VerticalId,
                            @SalesOrganizationId = inputDto.SalesOrganizationId,
                            @DistributionChannelId = inputDto.DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 0).Result.ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                return result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return result;
            }
        }

        #endregion

        #region call Recording Details

        public List<CallRecordingListOutputDto> GetCallRecordingList(DateTime fromDate, DateTime toDate, List<long> ZHIds, List<long> BDOIds, List<long> DealerIDs, long verticalId, long loginUserId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetCallRecordingList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<CallRecordingListOutputDto> result = new List<CallRecordingListOutputDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        var bdoIds = BDOIds != null && !BDOIds.Contains(0) ? String.Join(",", BDOIds) : "";
                        var zhIds = ZHIds != null && !ZHIds.Contains(0) ? String.Join(",", ZHIds) : "";
                        var dealerIds = DealerIDs != null && !DealerIDs.Contains(0) ? String.Join(",", DealerIDs) : "";
                        result = connection.Query<CallRecordingListOutputDto>("GetCallRecordingDetailsList", new
                        {
                            BDOIds = bdoIds,
                            ZHIds = zhIds,
                            dealerIds,
                            FromDate = fromDate,
                            Todate = toDate,
                            VerticalId = verticalId,
                            LoginUserId = loginUserId,
                            SalesOrganizationId,
                            DistributionChannelId

                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }


        public CallRecordingListOutputDto GetCallRecordedFileName(long AudioFileId)
        {
            _methodName = "GetCallRecordedFileName";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            CallRecordingListOutputDto result = new CallRecordingListOutputDto();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<CallRecordingListOutputDto>("GetCallRecordedFileName", new
                        {
                            AudioFileId
                        }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }


        public List<CallRecordingListOutputDto> GetSaudaCallRecordMappingList(DateTime fromDate, DateTime toDate, List<long> ZHIds, List<long> BDOIds, List<long> DealerIDs, long VerticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetSaudaCallRecordMappingList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<CallRecordingListOutputDto> result = new List<CallRecordingListOutputDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        var bdoIds = BDOIds != null && !BDOIds.Contains(0) ? String.Join(",", BDOIds) : "";
                        var zhIds = ZHIds != null && !ZHIds.Contains(0) ? String.Join(",", ZHIds) : "";
                        var dealerIds = DealerIDs != null && !DealerIDs.Contains(0) ? String.Join(",", DealerIDs) : "";
                        result = connection.Query<CallRecordingListOutputDto>("GetSaudaCallRecordMappingList", new
                        {
                            bdoIds,
                            zhIds,
                            dealerIds,
                            fromDate,
                            toDate,
                            VerticalId,
                            SalesOrganizationId,
                            DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                        result.ForEach(s =>
                        {
                            s.EncryptedId = UtilityHelper.ConvertToMd5(s.SaudaId.ToString(), SecurityConstants.EncryptionKey);
                        });
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        public async Task<CallRecordingDto> GetCallRecordedBasedOnSauda(long saudaId)
        {
            _methodName = "GetCallRecordedBasedOnSauda";
            var response = await GetById<CallRecordingDto>(ApiUrl.WebApiUrlGetSaudaCallRecordMappingAttachments, saudaId);
            return response;
        }

        #endregion


        #region Daily Booking Report 
        public async Task<List<SaudaOrderReportOutputDto>> GetDailyBookingReport(SaudaOrderReportInputputDto inputDto)
        {
            _methodName = "GetDailyBookingReport";
            var result = await GetListAsync<SaudaOrderReportOutputDto>(ApiUrl.WebApiUrlDailyBookingReport, inputDto);
            return result.ToList();
        }
        #endregion

        #region Filler Sku list
        public List<FillerSkuOutputDto> GetFillerSkuList(long verticalId, long SalesOrganizationId, long DistributionChannelId)
        {
            _methodName = "GetSalesRegisterList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<FillerSkuOutputDto> result = new List<FillerSkuOutputDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<FillerSkuOutputDto>("GetFillerSkuList", new
                        {
                            VerticalId = verticalId,
                            SalesOrganizationId,
                            DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region SKU Wise Premium Amount Report

        public List<SKUPremiumAmountReportDto> GetSKUWisePremiumAmountReport(ExcelReportFilterDto inputDto)
        {
            _methodName = "GetSKUWisePremiumAmountReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SKUPremiumAmountReportDto> result = new List<SKUPremiumAmountReportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<SKUPremiumAmountReportDto>("SP_Emami_SKUWisePremiumAmountReport", new
                        {
                            @SalesOrganizationId = inputDto.SalesOrganizationId,
                            @DistributionChannelId = inputDto.DistributionChannelId,
                            @VerticalId = inputDto.VerticalId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Truck Placement Tracker Report 

        public async Task<List<TruckPlacementTrackerDto>> GetTruckPlacementTrackerReport(ReportInputDto inputDto)
        {
            _methodName = "GetTruckPlacementTrackerReport";
            var result = await GetListAsync<TruckPlacementTrackerDto>(ApiUrl.WebApiUrlGetTruckPlacementTrackerReport, inputDto);
            return result.ToList();
        }

        #endregion

        #region SchemeGeographyReport

        public async Task<List<SchemeGeographyReportOutputDto>> GetSchemeGeographyDetailsReportAsync(SchemeGeographyReportInputputDto inputDto)
        {
            _methodName = "GetSchemeGeographyDetailsReport";
            var result = await GetListAsync<SchemeGeographyReportOutputDto>(ApiUrl.WebApiUrlSchemeGeographyReport, inputDto);
            return result.ToList();
        }

        #endregion

        #region DemandPlanBillingReport

        public async Task<List<DemandPlanBillingReportOutputDto>> GetDemandPlanBillingDetailsReportAsync(DemandPlanBillingReportInputputDto inputDto)
        {
            _methodName = "GetSchemeGeographyDetailsReport";
            var result = await GetListAsync<DemandPlanBillingReportOutputDto>(ApiUrl.WebApiUrlDemandPlanBillingReport, inputDto);
            return result.ToList();
        }

        #endregion


        #region GPSTrackingReport

        public List<GPSTrackingDto> GPSTrackingReport(GPSTrackingDto inputDto)
        {
            _methodName = "GPSTrackingReport";

            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<GPSTrackingDto> result = new List<GPSTrackingDto>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        result = connection.Query<GPSTrackingDto>("SP_GPSTrackingReport", new
                        {
                            inputDto.Id,
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;

        }

        #endregion


        #region Cross And Upsell 

        public async Task<List<SaudaCondtionalBookingReportOutputDto>> GetCrossAndUpsellSaudaOrderReportAsync(SaudaConditionalBookingReportInputDto inputDto)
        {
            _methodName = "CrossAndUpsellSaudaOrderReportAsync";
            var result = new List<SaudaCondtionalBookingReportOutputDto>();
            var stateIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateIds);
            var status = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StatusIds);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        result = (await connection.QueryAsync<SaudaCondtionalBookingReportOutputDto>(
                            "[dbo].[usp_GetCrossAndUpsellSaudaOrderReport]",
                            new
                            {
                                @RoleId = inputDto.RoleId,
                                @LoginUserId = inputDto.LoginUserId,
                                @FromDate = inputDto.FromDate,
                                @ToDate = inputDto.ToDate,
                                @StateIds = stateIds,
                                @StatusIds = status,
                                @VerticalId = inputDto.VerticalId,
                                @SalesOrganizationId = inputDto.SalesOrganizationId,
                                @DistributionChannelId = inputDto.DistributionChannelId
                            },
                            commandType: System.Data.CommandType.StoredProcedure,
                            commandTimeout: 0
                        )).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                }

                return result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return result;
            }
        }

        #endregion
    }
}