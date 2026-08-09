using GMCore.Helper;
using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Enums;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Sys = System.Configuration;
using System.IO;

using Google.Apis.AnalyticsData.v1beta.Data;
using Google.Apis.AnalyticsData.v1beta;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Data;

namespace Adani.Solution.MVC.ServiceClient
{
    public class ServiceClient : BaseClient
    {
        private const string ServiceName = "Role Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        public async Task<ResultDto> CallServiceNotification(LoginUserIdDto inputDto, string apiUrl)
        {
            _methodName = "AddOrUpdateVerticalDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new ResultDto();
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(apiUrl, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();

                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result.IsSuccess = true;
                        result.SuccessDto.Message = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.IsSuccess = false;
                        result.ErrorDto.Message = errorDtoResult.Message;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorDto.Message = ja[0][Settings.ResponseMessage].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.IsSuccess = false;
                result.ErrorDto.Message = Helper.GetResourceString("msg_DealerError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<DashboardDetailsDto> GetDashboardDetails(LoginUserIdDto inputDto)
        {
            var result = await GetByInputDto<DashboardDetailsDto>(ApiUrl.WebApiUrlGetDashboardDetails, inputDto);
            return result;
        }

        public async Task<GoogleAnalyticsDataDto> GetGoogleAnalyticsDataAsync(LoginUserIdDto input)
        {
            GoogleAnalyticsDataDto analyticsData = new GoogleAnalyticsDataDto();

            try
            {
                string keyFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/" + ConfigHelper.GoogleAnalyticsKeyFilePath));
                var webPropertyId = ConfigHelper.GoogleAnalyticsKeyFilePath;
                var mobilePropertyId = ConfigHelper.GoogleAnalyticsMobilePropertyId;

                var TotalLoginsByDistributor = await GetDistributorUsersCount(input);
                analyticsData.TotalLoginsByDistributor = (long)TotalLoginsByDistributor.TotalLoginsByDistributor;

                var TotalLoginsBySales = await GetSalesUsersCount(input);
                analyticsData.TotalLoginsBySales = (long)TotalLoginsBySales.TotalLoginsBySales;

                if (!string.IsNullOrEmpty(webPropertyId))
                {
                    var activeUserCount = await GetRealTimeActiveUsersCount(keyFilePath, webPropertyId);
                    var totalUserCount = await GetTotalUsersCount(keyFilePath, webPropertyId);
                    var recentUsersCount = await GetRecentUsersCount(keyFilePath, webPropertyId);

                    analyticsData.ActiveUserCount = (long)activeUserCount;
                    analyticsData.TotalEmployeesLoggedIn = (long)(ConfigHelper.GoogleAnalyticsPreviousUserCount + (long)totalUserCount);
                    //analyticsData.RecentUsersCount = (long)recentUsersCount;
                }

                if (!string.IsNullOrEmpty(mobilePropertyId))
                {
                    var activeUserCountforMobile = await GetRealTimeMobileActiveUsersCount(keyFilePath, mobilePropertyId);
                    var totalUserCountForMobile = await GetTotalMobileUsersCount(keyFilePath, mobilePropertyId);
                    var recentUsersCountForMobile = await GetRecentMobileUsersCount(keyFilePath, mobilePropertyId);

                    //analyticsData.RecentMobileUsersCount = (long)recentUsersCountForMobile;
                    analyticsData.ActiveUserCount = (long)activeUserCountforMobile;
                    analyticsData.TotalEmployeesLoggedIn = (long)(ConfigHelper.GoogleAnalyticsPreviousMobileUserCount + (long)totalUserCountForMobile);
                }

                return analyticsData;
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
                return analyticsData;
            }
        }

        //public async Task<long> GetRealTimeActiveUsersCount(string path, string propertyId)
        //{
        //    long activeUsersCount = 0;

        //    try
        //    {
        //        string credentialsPath = path;

        //        GoogleCredential credential;
        //        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        //        {
        //            credential = GoogleCredential.FromStream(stream);
        //        }

        //        var service = new AnalyticsDataService(new BaseClientService.Initializer
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = "Adani"
        //        });

        //        var request = new RunRealtimeReportRequest
        //        {
        //            Metrics = new List<Metric>
        //            {
        //                new Metric { Name = "activeUsers" }
        //            }
        //        };

        //        var response = service.Properties.RunRealtimeReport(request, propertyId).Execute();

        //        long activeUsers = 0;

        //        if (response.Rows.Count() > 0)
        //        {
        //            activeUsers = Convert.ToInt64(response.Rows[0].MetricValues.Select(x => x.Value).FirstOrDefault());
        //            activeUsersCount = activeUsers != null ? Convert.ToInt64(activeUsers) : 0;
        //        }

        //        return await Task.FromResult(activeUsersCount);
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {ex}";
        //        _logger.Error(message);
        //        return await Task.FromResult(activeUsersCount);
        //    }
        //}
        public async Task<long> GetRealTimeActiveUsersCount(string path, string propertyId)
        {
            long activeUsersCount = 0;
            //const string ServiceName = "AnalyticsService";
            //const string _methodName = nameof(GetRealTimeActiveUsersCount);

            try
            {
                GoogleCredential credential;
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream);
                }

                var service = new AnalyticsDataService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Adani"
                });

                var request = new RunRealtimeReportRequest
                {
                    Metrics = new List<Metric>
                {
                    new Metric { Name = "activeUsers" }
                }
                };

                var response = await service.Properties.RunRealtimeReport(request, propertyId).ExecuteAsync();

                if (response.Rows.Count > 0)
                {
                    var activeUsers = response.Rows[0].MetricValues.Select(x => x.Value).FirstOrDefault();
                    activeUsersCount = activeUsers != null ? Convert.ToInt64(activeUsers) : 0;
                }
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
            }

            return activeUsersCount;
        }

        public async Task<long> GetTotalUsersCount(string path, string propertyId)
        {
            long totalUsersCount = 0;

            try
            {
                string endDate = DateTime.Now.ToString("yyyy-MM-dd");
                int days = Convert.ToInt32(ConfigHelper.GoogleAnalyticsDataDaysInterval);
                string startDate = "2015-01-01";

                string credentialsPath = path;
                GoogleCredential credential;
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream);
                }

                var service = new AnalyticsDataService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential
                });

                var request = new RunReportRequest
                {
                    Property = propertyId,
                    DateRanges = new List<DateRange>
                    {
                        new DateRange { StartDate = startDate, EndDate = endDate }
                    },
                    Metrics = new List<Metric>
                    {
                        new Metric { Name = "totalUsers" }
                    }
                };

                var response = service.Properties.RunReport(request, propertyId).Execute();

                var usersCount = Convert.ToInt64(response.Rows[0].MetricValues.Select(x => x.Value).FirstOrDefault());
                totalUsersCount = usersCount != null ? Convert.ToInt64(usersCount) : 0;

                return await Task.FromResult(totalUsersCount);
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
                return await Task.FromResult(totalUsersCount);
            }
        }
        
        public async Task<long> GetRecentUsersCount(string path, string propertyId)
        {
            long totalUsersCount = 0;

            try
            {
                string endDate = DateTime.Now.ToString("yyyy-MM-dd");
                int days = Convert.ToInt32(ConfigHelper.GoogleAnalyticsDataDaysInterval);   
                string startDate = DateTime.Now.AddHours(-(24 * days)).ToString("yyyy-MM-dd");

                string credentialsPath = path;
                GoogleCredential credential;
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream);
                }

                var service = new AnalyticsDataService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Adani"
                });

                var request = new RunReportRequest
                {
                    Property = propertyId,
                    DateRanges = new List<DateRange>
                    {
                        new DateRange { StartDate = startDate, EndDate = endDate }
                    },
                    Metrics = new List<Metric>
                    {
                        new Metric { Name = "activeUsers" }
                    }
                };

                var response = service.Properties.RunReport(request, propertyId).Execute();

                var usersCount = Convert.ToInt64(response.Rows[0].MetricValues.Select(x => x.Value).FirstOrDefault());
                totalUsersCount = usersCount != null ? Convert.ToInt64(usersCount) : 0;

                return await Task.FromResult(totalUsersCount);
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
                return await Task.FromResult(totalUsersCount);
            }
        }
        
        public async Task<long> GetTotalMobileUsersCount(string path, string propertyId)
        {
            long totalUsersCount = 0;

            try
            {
                string endDate = DateTime.Now.ToString("yyyy-MM-dd");
                string startDate = "2015-01-01";

                string credentialsPath = path;
                GoogleCredential credential;
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream);
                }

                var service = new AnalyticsDataService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Adani"
                });

                var request = new RunReportRequest
                {
                    Property = propertyId,
                    DateRanges = new List<DateRange>
                    {
                        new DateRange { StartDate = startDate, EndDate = endDate }
                    },
                    Dimensions = new List<Dimension>
                    {
                        new Dimension { Name = "deviceCategory" }
                    },
                    Metrics = new List<Metric>
                    {
                        new Metric { Name = "totalUsers" }
                    },
                    DimensionFilter = new FilterExpression
                    {
                        Filter = new Filter
                        {
                            FieldName = "deviceCategory",
                            StringFilter = new StringFilter
                            {
                                MatchType = "EXACT",
                                Value = "mobile"
                            }
                        }
                    }
                };

                var response = service.Properties.RunReport(request, propertyId).Execute();

                var usersCount = Convert.ToInt64(response.Rows[0].MetricValues.Select(x => x.Value).FirstOrDefault());
                totalUsersCount = usersCount != null ? Convert.ToInt64(usersCount) : 0;

                return await Task.FromResult(totalUsersCount);
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
                return await Task.FromResult(totalUsersCount);
            }
        }
        
        public async Task<long> GetRealTimeMobileActiveUsersCount(string path, string propertyId)
        {
            long activeUsersCount = 0;

            try
            {
                string credentialsPath = path;

                GoogleCredential credential;
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream);
                }

                var service = new AnalyticsDataService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Adani"
                });

                var request = new RunRealtimeReportRequest
                {
                    Dimensions = new List<Dimension>
                    {
                        new Dimension { Name = "deviceCategory" }
                    },
                    Metrics = new List<Metric>
                    {
                        new Metric { Name = "activeUsers" }
                    },
                    DimensionFilter = new FilterExpression
                    {
                        Filter = new Filter
                        {
                            FieldName = "deviceCategory",
                            StringFilter = new StringFilter
                            {
                                MatchType = "EXACT",
                                Value = "mobile"
                            }
                        }
                    }
                };

                var response = service.Properties.RunRealtimeReport(request, propertyId).Execute();

                long activeUsers = 0;

                if (response.Rows.Count() > 0)
                {
                    activeUsers = Convert.ToInt64(response.Rows[0].MetricValues.Select(x => x.Value).FirstOrDefault());
                    activeUsersCount = activeUsers != null ? Convert.ToInt64(activeUsers) : 0;
                }

                return await Task.FromResult(activeUsersCount);
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
                return await Task.FromResult(activeUsersCount);
            }
        }
        
        public async Task<long> GetRecentMobileUsersCount(string path, string propertyId)
        {
            long totalUsersCount = 0;

            try
            {
                string endDate = DateTime.Now.ToString("yyyy-MM-dd");
                int days = Convert.ToInt32(ConfigHelper.GoogleAnalyticsDataDaysInterval);
                string startDate = DateTime.Now.AddHours(-(24 * days)).ToString("yyyy-MM-dd");

                string credentialsPath = path;
                GoogleCredential credential;
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream);
                }

                var service = new AnalyticsDataService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Adani"
                });

                var request = new RunReportRequest
                {
                    Property = propertyId,
                    DateRanges = new List<DateRange>
                    {
                        new DateRange { StartDate = startDate, EndDate = endDate }
                    },
                    Dimensions = new List<Dimension>
                    {
                        new Dimension { Name = "deviceCategory" }
                    },
                    Metrics = new List<Metric>
                    {
                        new Metric { Name = "activeUsers" }
                    },
                    DimensionFilter = new FilterExpression
                    {
                        Filter = new Filter
                        {
                            FieldName = "deviceCategory",
                            StringFilter = new StringFilter
                            {
                                MatchType = "EXACT",
                                Value = "mobile"
                            }
                        }
                    }
                };

                var response = service.Properties.RunReport(request, propertyId).Execute();

                var usersCount = Convert.ToInt64(response.Rows[0].MetricValues.Select(x => x.Value).FirstOrDefault());
                totalUsersCount = usersCount != null ? Convert.ToInt64(usersCount) : 0;

                return await Task.FromResult(totalUsersCount);
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {ex}";
                _logger.Error(message);
                return await Task.FromResult(totalUsersCount);
            }
        }

        public async Task<GoogleAnalyticsDataDto> GetDistributorUsersCount(LoginUserIdDto input)
        {
            var result = await GetByInputDto<GoogleAnalyticsDataDto>(ApiUrl.WebApiUrlGetUserLoginInfo, input);
            return result;
        }

     
        public async Task<GoogleAnalyticsDataDto> GetSalesUsersCount(LoginUserIdDto input)
        {
            var result = await GetByInputDto<GoogleAnalyticsDataDto>(ApiUrl.WebApiUrlGetSalesLoginInfo, input);
            return result;
        }

    }
}