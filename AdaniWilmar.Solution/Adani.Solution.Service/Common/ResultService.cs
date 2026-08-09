using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.DTO.Enums;
using Dapper;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Reflection;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TagLib.Ape;

namespace Adani.Solution.Service.Common
{
    public interface IResultService
    {
        ResultDto ErrorMessage(string errorCode);
        ResultDto SuccessMessage(string successCode);
        ResultDto SuccessObject(Object resultObject);
        ResultDto SuccessMessageWitObject(Object resultObject, string successCode);
        ResultDto ErrorMessageWitObject(Object resultObject, string errorCode);
        bool UserIsAcive(long userID);
        decimal GetSkuQuanityRate(long quantityTypeId, decimal quantity, decimal ratePerMt, decimal litreConversion);
        decimal ConvertCasetoMetricTon(decimal quantity, long skuId);
        //decimal ConvertCasetoMetricTon(decimal quantity, decimal skuQuantity, decimal numberOfPcs, long uomId, decimal ltrConversion);
        decimal ConvertMetricTontoCase(decimal quantity, decimal skuQuantity, decimal numberOfPcs, long uomId, decimal ltrConversion);
        decimal ConvertNostoMetricTon(decimal quantity, long skuId);
        decimal ConvertMetricTontoNosOrCase(decimal quantity, long skuId, long uomId);
        string GetBulletinMediaPath(int contentTypeId, string fileName);
        string GetCustomerLedgerPath(string fileName);
        string GetCompetitorFilePath(string fileName);
        string GetWholesellerFilePath(string fileName);
        decimal ConvertRatePerMetricToRatePerCase(long skuId, decimal ratePerMt);
        decimal ConvertRatePerMetricToRatePerCaseForExcel(long skuId, decimal ratePerMt, List<SkuUomMapping> skuUomMapping, List<Sku> sku);

        bool IsSMS();
        bool IsEmail();
        bool IsPushNotification();

        decimal ConvertCasetoMetricTonSaudaBooking(List<ConvertCasetoMetricTon> inputList);
        HttpResponseMessage PostAsyncWithBaicAuthentication(string functionUrl, object model, bool isDarwinboxAPI = false);
        decimal SaudaAvailableQuantityCheck(string saudaNumber, long SkuId);
        decimal AvailableSaudaLimit(long dealerId, decimal UserSaudaLimit, long SalesOrganizationId, long DistributionChannelId, long DivisionId);
        decimal ConvertCasetoMetricTonWithoutDB(decimal quantity, long skuId, List<SkuUomMappingDto> skuUomMappings);
        decimal ConvertMetricTonToQuantityCase(decimal metricTon, long skuId);
        Tuple<bool, string> IsSaudaConditionalBookingValid(SaudaInputDto inputDto);
        long GetDiscountId(SaudaInputDto inputDto, SaudaOrderInputDto orderInputDto);
        decimal GetSaudaBookedQuantityForCurrentDate(SaudaInputDto inputDto, long oilTypeId);
        decimal GetSaudaBookedQuantityForCurrentDateByDealers(SaudaInputDto inputDto, long oilTypeId, List<long> DealerList);

        bool IsSalesAreaBookingValid(SaudaInputDto inputDto);
        decimal CalculateAutomatedDiscount(decimal discountAmount, long convertFromSkuId, long convertToSkuId);

        decimal GetSaudaBookedQuantityForCurrentDateByDealersByDateRangeIsReportingtoAllocation(
    SaudaInputDto inputDto,
    long oilTypeId,
    List<long> dealerList,
    DateTime validFrom,
    DateTime validTo);

        decimal GetSaudaBookedQuantityForCurrentDateByDealersByDateRangeIsNotReportingtoAllocation(
    SaudaInputDto inputDto,
    long oilTypeId,
    List<long> dealerList,
    DateTime validFrom,
    DateTime validTo);

        decimal GetSaudaBookedQuantityForCurrentDateByDateRangeIsNotReportingtoAllocation(
    SaudaInputDto inputDto,
    long oilTypeId,
    DateTime validFrom,
    DateTime validTo);
        decimal GetSaudaBookedQuantityForCurrentDateByDateRangeIsReportingtoAllocation(
    SaudaInputDto inputDto,
    long oilTypeId,
    DateTime validFrom,
    DateTime validTo);
    }

    public class ResultService : IResultService
    {
        private readonly IAdaniContext _adaniContext;
        private readonly ILogger _logger = Logging.GetLogger("Result Service");
        private const string ServiceName = "Result Service";
        //private string _methodName;

        public ResultService() { }

        public ResultService(IAdaniContext emamiContext)
        {
            try
            {
                _adaniContext = emamiContext;
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Result Service", exception);
            }
        }
        public ResultDto ErrorMessage(string message)
        {
            var resultDto = new ResultDto
            {
                IsSuccess = false
            };
            resultDto.ErrorDto.Message = message;
            return resultDto;
        }
        public ResultDto SuccessMessage(string message)
        {
            var resultDto = new ResultDto
            {
                IsSuccess = true
            };
            resultDto.SuccessDto.Response = message;
            return resultDto;
        }
        public ResultDto SuccessObject(Object resultObject)
        {
            var resultDto = new ResultDto
            {
                IsSuccess = true
            };
            resultDto.SuccessDto.Response = resultObject;
            return resultDto;
        }

        public ResultDto SuccessMessageWitObject(Object resultObject, string message)
        {
            var resultDto = new ResultDto
            {
                IsSuccess = true,
            };
            resultDto.SuccessDto.Message = message;
            resultDto.SuccessDto.Response = resultObject;
            return resultDto;
        }

        public ResultDto ErrorMessageWitObject(Object resultObject, string message)
        {
            var resultDto = new ResultDto
            {
                IsSuccess = false,
            };
            resultDto.ErrorDto.Message = message;
            resultDto.ErrorDto.Response = resultObject;
            return resultDto;
        }

        public bool UserIsAcive(long userID)
        {
            var userCount = _adaniContext.Users.AsNoTracking().Count(_ => _.Id == userID && _.IsActive);
            if (userCount > 0)
                return true;
            else
                return false;
        }

        public bool IsSMS()
        {
            var IsSMS = _adaniContext.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsSMS).Select(_ => _.Value).Single();
            if (IsSMS.Equals("1") || IsSMS.Equals("True"))
                return true;
            else
                return false;
        }

        public bool IsEmail()
        {
            var IsEmail = _adaniContext.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsEMAIL).Select(_ => _.Value).Single();
            if (IsEmail.Equals("1") || IsEmail.Equals("True"))
                return true;
            else
                return false;
        }
        public bool IsPushNotification()
        {
            var IsPushNotification = _adaniContext.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsPushNotification).Select(_ => _.Value).Single();
            if (IsPushNotification.Equals("1") || IsPushNotification.Equals("True"))
                return true;
            else
                return false;
        }
        public decimal GetSkuQuanityRate(long quantityTypeId, decimal quantity, decimal ratePerMt, decimal litreConversion)
        {
            var quantityRate = (decimal)0;
            if (quantityTypeId == (int)DTO.Enums.Uom.Ltr)
            {
                //Litre conversion From SKU 
                //var oneLrRate = ratePerMt / litreConversion;

                //1000 kg x 1 L / 0.92 kg = 1087 L approx.
                //litreConversion = Convert.ToDecimal(1000 / 0.91);  //Oiltype KgConversion
                var oneLitreRate = ratePerMt / litreConversion;
                quantityRate = quantity * oneLitreRate;
            }
            else
            {
                var oneKgRate = ratePerMt / 1000;
                quantityRate = quantity * oneKgRate;
            }
            return quantityRate;
        }


        /// <summary>
        /// Method to quantity from case to metric tone
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public decimal ConvertCasetoMetricTon(decimal quantity, long skuId)
        {
            var metricTone = (decimal)0;
            using (var _adaniContext = new AdaniContext())
            {
                var skuContext = _adaniContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                if (skuContext != null)
                {
                    var skuUomContext = _adaniContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId);
                    // conversionFactor1  -  UMREZ ,  mtConversionFactor2 - UMREN
                   // any material from its corresponding UOM to MT -> 
                   //quantity* ConversionFactor2 / ConversionFactor1

                    if (skuUomContext != null)
                    {
                        //if (skuUomContext.UomId == (int)DTO.Enums.Uom.Case) // CAR
                        //{
                        //    metricTone = quantity / skuUomContext.ConversionFactor1 * skuUomContext.ConversionFactor2;
                        //}
                        //else if (skuUomContext.UomId == (int)DTO.Enums.Uom.EA)
                        //{
                            metricTone = quantity * skuUomContext.ConversionFactor2 / skuUomContext.ConversionFactor1;
                       // }
                    }
                }
            }
            return metricTone;
        }

        /// <summary>
        /// Method to convert metric tone to quantity case
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public decimal ConvertMetricTonToQuantityCase(decimal metricTon, long skuId)
        {
            var quantity = (decimal)0;
            using (var _adaniContext = new AdaniContext())
            {
                var skuContext = _adaniContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                if (skuContext != null)
                {
                    var skuUomContext = _adaniContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId);

                    if (skuUomContext != null)
                    {
                        quantity = metricTon * skuUomContext.ConversionFactor1 / skuUomContext.ConversionFactor2;
                    }
                }
            }
            return quantity;
        }

        /// <summary>
        /// Method to quantity from case to metric tone
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public decimal ConvertCasetoMetricTonWithoutDB(decimal quantity, long skuId, List<SkuUomMappingDto> skuUomMappings)
        {
            var metricTone = (decimal)0;
            using (var _adaniContext = new AdaniContext())
            {
                //var skuContext = _adaniContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                if (skuUomMappings != null)
                {
                    var skuUomContext = skuUomMappings.FirstOrDefault(_ => _.SkuId == skuId);
                    // conversionFactor1  -  UMREZ ,  mtConversionFactor2 - UMREN
                    // any material from its corresponding UOM to MT -> 
                    //quantity* ConversionFactor2 / ConversionFactor1
                    if (skuUomContext != null)
                    {
                        //if (skuUomContext.UomId == (int)DTO.Enums.Uom.Case) // CAR
                        //{
                        //    metricTone = quantity / skuUomContext.ConversionFactor1 * skuUomContext.ConversionFactor2;
                        //}
                        //else if (skuUomContext.UomId == (int)DTO.Enums.Uom.EA)
                        //{
                            metricTone = quantity * skuUomContext.ConversionFactor2 / skuUomContext.ConversionFactor1;
                        //}
                    }
                }
            }
            return metricTone;
        }
        /// <summary>
        /// Method to quantity from case to metric tone
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public decimal ConvertCasetoMetricTonSaudaBooking(List<ConvertCasetoMetricTon> inputList)
        {
            var metricTone = (decimal)0;
            var skuIds = inputList.Select(s => s.SkuId).Distinct().ToList();
            var skuDatas = _adaniContext.Skus.AsNoTracking()
                    .Where(w => skuIds.Contains(w.Id))
                    .Select(s => new ConvertCasetoMetricTonSku
                    {
                        Id = s.Id,
                        Quantity = s.Quantity,
                        UomId = s.UomId,
                        //  LitreConversion = s.OilType.LitreConversion
                    }).Distinct().ToList();

            var skuUomMappingDatas = _adaniContext.SkuUomMapping.AsNoTracking()
                .Where(_ => skuIds.Contains(_.SkuId))
                .Select(s => new ConvertCasetoMetricTonSkuUom
                {
                    SkuId = s.SkuId,
                    ConversionFactor = s.ConversionFactor,
                    UomId = s.UomId,
                    RelationUomId = s.RelationUomId
                }).Distinct().ToList();

            if (skuDatas.IsAny() && inputList.IsAny())
            {
                foreach (var item in inputList)
                {
                    var skuContext = skuDatas.FirstOrDefault(_ => _.Id == item.SkuId);
                    if (skuContext != null)
                    {
                        var numberOfPcs = (decimal)0;
                        if (skuUomMappingDatas.IsAny())
                        {
                            var skuUomContext = skuUomMappingDatas.FirstOrDefault(_ => _.SkuId == item.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                            if (skuUomContext != null)
                            {
                                numberOfPcs = skuUomContext.ConversionFactor;
                            }
                        }
                        var totalQty = (numberOfPcs * item.Quantity) * skuContext.Quantity;

                        var quantityTypeId = skuContext.UomId;
                        var ltrConversion = skuContext.LitreConversion;
                        if (quantityTypeId == (int)DTO.Enums.Uom.Ltr)
                        {
                            metricTone = (metricTone + totalQty / ltrConversion);
                        }
                        else
                        {
                            metricTone = (metricTone + totalQty / 1000);
                        }
                    }
                }
            }
            return metricTone;
        }

        //public decimal ConvertCasetoMetricTon(decimal quantity, decimal skuQuantity, decimal numberOfPcs, long uomId, decimal ltrConversion)
        //{
        //    var metricTone = (decimal)0;
        //    var totalQty = (numberOfPcs * quantity) * skuQuantity;
        //    //if (uomId == (int)DTO.Enums.Uom.Ltr)
        //    //{
        //    //    metricTone = totalQty / ltrConversion;
        //    //}
        //    //else
        //    //{
        //    //    metricTone = totalQty / 1000;
        //    //}
        //    return totalQty;
        //}

        public decimal ConvertMetricTontoCase(decimal quantity, decimal skuQuantity, decimal numberOfPcs, long uomId, decimal ltrConversion)
        {
            var caseValue = (decimal)0;
            if (uomId == (int)DTO.Enums.Uom.Ltr)
            {
                caseValue = ltrConversion / (skuQuantity * numberOfPcs);
            }
            else
            {
                caseValue = 1000 / (skuQuantity * numberOfPcs);
            }

            return caseValue;
        }

        //public decimal ConvertMetricTontoCase(decimal quantity, long skuId)
        //{
        //    var caseValue = (decimal)0;
        //    var skuContext = _adaniContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
        //    if (skuContext != null)
        //    {
        //        var numberOfPcs = (decimal)0;
        //        var skuUomContext = _adaniContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
        //        if (skuUomContext != null)
        //        {
        //            numberOfPcs = skuUomContext.ConversionFactor;
        //        }

        //        var quantityTypeId = skuContext.UomId;
        //        var ltrConversion = skuContext.OilType.LitreConversion;
        //        if (quantityTypeId == (int)DTO.Enums.Uom.Ltr)
        //        {
        //            caseValue = quantity * ltrConversion;
        //        }
        //        else
        //        {
        //            caseValue = quantity * 1000;
        //        }

        //        caseValue = (skuContext.Quantity * numberOfPcs) * caseValue;
        //    }

        //    return caseValue;
        //}




        /// <summary>
        /// Method to quantity from case to metric tone
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public decimal ConvertNostoMetricTon(decimal quantity, long skuId)
        {
            var metricTone = (decimal)0;
            using (var _adaniContext = new AdaniContext())
            {
                var skuContext = _adaniContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                if (skuContext != null)
                {
                    var totalQty = quantity * skuContext.Quantity;
                    var quantityTypeId = skuContext.UomId;
                    var ltrConversion = 0;
                    //skuContext.OilType.LitreConversion;
                    if (quantityTypeId == (int)DTO.Enums.Uom.Ltr)
                    {
                        metricTone = totalQty / ltrConversion;
                    }
                    else
                    {
                        metricTone = totalQty / 1000;
                    }
                }
            }
            //metricTone = Convert.ToDecimal(String.Format(Constants.DefaultDecimalPlacesForMT, metricTone));
            return metricTone;
        }

        /// <summary>
        /// Method to quantity from  metric tone to case or nos
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public decimal ConvertMetricTontoNosOrCase(decimal quantity, long skuId, long uomId)
        {
            var caseQuantity = (decimal)0;
            var nosQuantity = (decimal)0;
            using (var _adaniContext = new AdaniContext())
            {
                var skuContext = _adaniContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                if (skuContext != null)
                {
                    var skuUomMappingNos = _adaniContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                    if (skuUomMappingNos != null)
                    {
                        nosQuantity = quantity * skuUomMappingNos.ConversionFactor;
                    }
                    var skuUomMapping = _adaniContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                    if (skuUomMapping != null && uomId == (int)DTO.Enums.Uom.Case)
                    {
                        caseQuantity = nosQuantity / skuUomMappingNos.ConversionFactor;
                    }
                }
            }
            return uomId == (int)DTO.Enums.Uom.Case ? caseQuantity : nosQuantity;
        }



        public string GetBulletinMediaPath(int contentTypeId, string fileName)
        {
            var folderName = Enum.GetName(typeof(DTO.Enums.ContentType), contentTypeId);

            var filePath = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = string.Concat(ConfigurationManager.AppSettings["WebsiteUrl"],
                    ConfigurationManager.AppSettings["UploadMediaPath"], "/", folderName, "/", fileName);

            }
            return filePath;
        }

        public string GetCustomerLedgerPath(string fileName)
        {
            var folderName = ConfigurationManager.AppSettings["CustomerLedgerFoldername"];

            var filePath = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = string.Concat(ConfigurationManager.AppSettings["SAPApiUrl"],
                    ConfigurationManager.AppSettings["UploadAttachments"], "/", folderName, "/", fileName);
            }
            return filePath;
        }

        public string GetCompetitorFilePath(string fileName)
        {
            var folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.Competitor);

            var filePath = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = string.Concat(ConfigurationManager.AppSettings["ApiUrl"],
                    ConfigurationManager.AppSettings["UploadAttachments"], "/", folderName, "/", fileName);
            }
            return filePath;
        }

        public string GetWholesellerFilePath(string fileName)
        {
            var folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.ProspectiveDealer);

            var filePath = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = string.Concat(ConfigurationManager.AppSettings["ApiUrl"],
                    ConfigurationManager.AppSettings["UploadAttachments"], "/", folderName, "/", fileName);
            }
            return filePath;
        }

        public decimal ConvertRatePerMetricToRatePerCase(long skuId, decimal ratePerMt)
        {
            var noofPiecesperCase = (decimal)0;
            var litreConversion = (decimal)0;
            var quantity = (decimal)0;
            var uomId = 0L;
            var costPerCase = (decimal)0;
            using (var _adaniContext = new AdaniContext())
            {
                var skuUomContext = _adaniContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                if (skuUomContext != null)
                {
                    noofPiecesperCase = skuUomContext.ConversionFactor;
                }
                var skuContext = _adaniContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                if (skuContext != null && skuContext.OilType != null)
                {
                    // litreConversion = skuContext.OilType.LitreConversion;
                    uomId = Convert.ToInt64(skuContext.UomId);
                    quantity = skuContext.Quantity;
                    costPerCase = GetSkuQuanityRate(uomId, quantity, ratePerMt, litreConversion);
                    costPerCase = noofPiecesperCase * costPerCase;
                }
            }
            return costPerCase;
        }

        public decimal ConvertRatePerMetricToRatePerCaseForExcel(long skuId, decimal ratePerMt, List<SkuUomMapping> skuUomMapping, List<Sku> sku)
        {
            var noofPiecesperCase = (decimal)0;
            var litreConversion = (decimal)0;
            var quantity = (decimal)0;
            var uomId = 0L;
            var costPerCase = (decimal)0;
            using (var _adaniContext = new AdaniContext())
            {
                var skuUomContext = skuUomMapping.FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                if (skuUomContext != null)
                {
                    noofPiecesperCase = skuUomContext.ConversionFactor;
                }
                var skuContext = sku.FirstOrDefault(_ => _.Id == skuId);
                if (skuContext != null && skuContext.OilType != null)
                {
                    // litreConversion = skuContext.OilType.LitreConversion;
                    uomId = Convert.ToInt64(skuContext.UomId);
                    quantity = skuContext.Quantity;
                    costPerCase = GetSkuQuanityRate(uomId, quantity, ratePerMt, litreConversion);
                    costPerCase = noofPiecesperCase * costPerCase;
                }
            }
            return costPerCase;
        }

        //private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    var x = new X509Certificate2();
        //    if (sslPolicyErrors == SslPolicyErrors.None) return true;
        //    return false;
        //}

        public HttpResponseMessage PostAsyncWithBaicAuthentication(string functionUrl, object model, bool isDarwinboxAPI = false)
        {
            var responseMessage = new HttpResponseMessage();
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(model)}");
            try
            {

                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        _logger.Info($"Url : {functionUrl}");
                        client.DefaultRequestHeaders.Add("ContentType", "application/json");
                        SetBasicAuthenticationHeaderValue(client, isDarwinboxAPI);
                        string json = JsonConvert.SerializeObject(model);
                        var content = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                        var webApiUrl = functionUrl;
                        responseMessage = client.PostAsync(functionUrl, content).Result;
                        _logger.Info($"Response : {responseMessage.ToString()}");

                    }
                }
            }
            catch (Exception e)
            {
                _logger.Info($"Exception : {e.ToString()}");
            }
            return responseMessage;
        }

        private static void SetBasicAuthenticationHeaderValue(HttpClient client, bool isDarwinboxAPI)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            string credentials = string.Empty;
            if (isDarwinboxAPI)
            {
                credentials = $"{ConsoleSettings.DarwinboxUsername}:{ConsoleSettings.DarwinboxPassword}";
            }
            else
            {
                credentials = $"{ConsoleSettings.HanaUsername}:{ConsoleSettings.HanaPassword}";
            }

            var byteArray = Encoding.ASCII.GetBytes(credentials);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public decimal SaudaAvailableQuantityCheck(string saudaNumber, long SkuId)
        {
            var saudaSkuDetailsDto = new List<SaudaSkuDetailsDto>();
            using (var _adaniContext = new AdaniContext())
            {
                var saudaContext = _adaniContext.Sauda.AsNoTracking()
                    .FirstOrDefault(_ => _.SaudaNumber == saudaNumber);
                //var LiftingRequestIds = _adaniContext.LiftingRequest.AsNoTracking()
                //    .Where(_ => _.SaudaNumber == saudaNumber).Select(s => s.Id).ToList();
                //if (LiftingRequestIds != null && LiftingRequestIds.IsAny())
                //{
                var LiftingRequestDetailsContext = _adaniContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SaudaNumber == saudaNumber && _.SkuId == SkuId).ToList();
                if (LiftingRequestDetailsContext != null && LiftingRequestDetailsContext.Any())
                {
                    var salesOrderQuntityresult = LiftingRequestDetailsContext
                      .GroupBy(l => l.SaudaOrderId)
                      .Select(cl => new
                      {
                          SkuId = cl.First().SkuId,
                          LiftingQuantityCase = cl.Sum(c => c.LiftingQuantityCase),
                          SaudaOrderId = cl.FirstOrDefault().SaudaOrderId
                      });
                    //var allSkuIds = salesOrderQuntityresult.Select(x => x.SkuId).Distinct();
                    var _SkuContext = _adaniContext.Skus.AsNoTracking()/*.Where(_ => allSkuIds.Contains(_.Id))*/.ToList();

                    var saudaOrdersContext = _adaniContext.SaudaOrders.Where(_ => _.SaudaId == saudaContext.Id && _.StatusId != (int)DTO.Enums.Status.Completed && _.SkuId == SkuId);
                    if (saudaOrdersContext != null && saudaOrdersContext.Any())
                    {
                        foreach (var saudaorder in saudaOrdersContext)
                        {
                            var salesorder = salesOrderQuntityresult.FirstOrDefault(_ => _.SkuId == saudaorder.SkuId && _.SaudaOrderId == saudaorder.Id);

                            var sku = _SkuContext.FirstOrDefault(_ => _.Id == saudaorder.SkuId);
                            var saudaSkuDetails = new SaudaSkuDetailsDto
                            {
                                SkuCode = sku.SkuCode,
                                SkuName = sku.SkuName,
                                SkuId = sku.Id,
                                AvailableQuantity = saudaorder.BidQuantityCase - (salesorder != null ? salesorder.LiftingQuantityCase : 0),
                                SaudaOrderId = saudaorder.Id
                            };
                            saudaSkuDetailsDto.Add(saudaSkuDetails);
                        }
                    }
                }
                //}
                else
                {
                    var saudaOrdersContext = _adaniContext.SaudaOrders.Where(_ => _.SaudaId == saudaContext.Id && _.StatusId != (int)DTO.Enums.Status.Completed && _.SkuId == SkuId);
                    if (saudaOrdersContext != null && saudaOrdersContext.IsAny())
                    {
                        var saudaSkuDetails = saudaOrdersContext.Select(a => new SaudaSkuDetailsDto
                        {
                            SkuCode = _adaniContext.Skus.FirstOrDefault(_ => _.Id == a.SkuId).SkuCode,
                            SkuName = _adaniContext.Skus.FirstOrDefault(_ => _.Id == a.SkuId).SkuName,
                            SkuId = a.SkuId,
                            AvailableQuantity = a.BidQuantityCase,
                            SaudaOrderId = a.Id
                        }).ToList();

                        saudaSkuDetailsDto.AddRange(saudaSkuDetails);
                    }
                }
            }
            var availableQuantity = saudaSkuDetailsDto.FirstOrDefault(a => a.SkuId == SkuId).AvailableQuantity > 0 ? saudaSkuDetailsDto.FirstOrDefault(a => a.SkuId == SkuId).AvailableQuantity : 0;
            return availableQuantity;
        }

        public decimal AvailableSaudaLimit(long dealerId, decimal UserSaudaLimit, long SalesOrganizationId, long DistributionChannelId, long DivisionId)
        {
            decimal availableSaudaLimit = 0;
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            using (var _adaniContext = new AdaniContext())
            {
                var SaudaOutstandingContext = (from s in _adaniContext.Sauda.AsNoTracking()
                                               join so in _adaniContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                               where s.UserId == dealerId
                                               && s.SaudaNumber == null && s.StatusId == (int)DTO.Enums.Status.Pending
                                               && s.SalesOrganizationId == SalesOrganizationId && s.DistributionChannelId == DistributionChannelId
                                               && s.DivisionId == DivisionId
                                               select new { BidQuantity = so.BidQuantity }
                                              );

                var SaudaOutstandingquantity = SaudaOutstandingContext != null && SaudaOutstandingContext.Any() ? SaudaOutstandingContext.Sum(s => s.BidQuantity) : 0;

                var todayCreatedSaudaNumbers = (from s in _adaniContext.Sauda.AsNoTracking()
                                                join so in _adaniContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                                where s.UserId == dealerId
                                                && !string.IsNullOrEmpty(s.SaudaNumber) && DbFunctions.TruncateTime(s.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                                                 && s.SalesOrganizationId == SalesOrganizationId && s.DistributionChannelId == DistributionChannelId
                                               && s.DivisionId == DivisionId
                                                select new { BidQuantity = so.BidQuantity, SaudaNumber = s.SaudaNumber }
                                              ).ToList();

                var saudaNumbers = todayCreatedSaudaNumbers.Select(s => s.SaudaNumber).ToList();
                var todayCreatedSaudaNumbersExistInPendingContracts = _adaniContext.PendingContracts.AsNoTracking().Where(p => saudaNumbers.Contains(p.SaudaNumber)).Select(_ => _.SaudaNumber).ToList();
                todayCreatedSaudaNumbers.RemoveAll(_ => todayCreatedSaudaNumbersExistInPendingContracts.Contains(_.SaudaNumber));
                var quantity = todayCreatedSaudaNumbers != null && todayCreatedSaudaNumbers.Any() ? todayCreatedSaudaNumbers.Sum(a => a.BidQuantity) : 0;
                var existingQuantity = SaudaOutstandingquantity + quantity;
                var pendingContracttablevalue = _adaniContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == dealerId && _.SalesOrgId == SalesOrganizationId && _.DistChnlId == DistributionChannelId && _.DivisionId == DivisionId).ToList().IsAny() ? _adaniContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == dealerId && _.SalesOrgId == SalesOrganizationId && _.DistChnlId == DistributionChannelId && _.DivisionId == DivisionId).Select(_ => _.SaudaQuantity).Sum() : 0;

                availableSaudaLimit = UserSaudaLimit - existingQuantity - pendingContracttablevalue;
            }

            return availableSaudaLimit;
        }

        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                Type propType = property.PropertyType;
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propType = Nullable.GetUnderlyingType(propType);
                }

                dataTable.Columns.Add(property.Name, propType);
            }

            foreach (T item in items)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public Tuple<bool, string> IsSaudaConditionalBookingValid(SaudaInputDto inputDto)
        {
            var methodName = "IsSaudaConditionalBookingValid";
            bool isBookingValid = false;
            bool isConditionValid = false;
            string errorMessage = string.Empty;
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var mandatorySkuMappings = new List<SaudaConditionalBookingMandatorySkuPricingDto>();

            try
            {
                if (inputDto == null)
                   return new Tuple<bool, string>(isBookingValid, Constants.InvalidRequest);

                var selectedSkuIds = inputDto.SaudaOrders.Select(o => o.SkuId).ToList();
                var plantId = inputDto.SaudaOrders.Select(o => o.PlantId).FirstOrDefault();

                var customerStateId = _adaniContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == inputDto.DealerId)?.StateId ?? 0;

                var bookingConfigurations = (from config in _adaniContext.SaudaConditionalBookingConfigurations.AsNoTracking()
                                             join mapping in _adaniContext.SaudaConditionalBookingEssentialSkuMappings.AsNoTracking()
                                                 on config.Id equals mapping.SaudaConditionalConfigurationId
                                             join zoneStatemapping in _adaniContext.SaudaConditionalBookingZoneStateMappings.AsNoTracking()
                                                 on config.Id equals zoneStatemapping.SaudaConditionalConfigurationId
                                             where config.SalesOrganizationId == inputDto.SalesOrganizationId &&
                                             config.DistributionChannelId == inputDto.DistributionChannelId && config.DivisionId == inputDto.DivisionId &&                                                 
                                             config.IsActive && mapping.IsActive && config.StartDate <= DateTime.Now &&
                                             config.EndDate >= DateTime.Now && zoneStatemapping.StateId == customerStateId
                                             select new { mapping, config })
                                             .OrderByDescending(_ => _.config.Id).ThenBy(_ => _.config.CreatedDate)
                                             .Distinct().ToList();

                if (bookingConfigurations.Any())
                {
                    var configurationDataList = bookingConfigurations.Select(_ => _.mapping).ToList();

                    foreach (var config in configurationDataList)
                    {
                        var essentialSkuIds = config.EssentialSkuId.Split(',').ToList().ConvertAll(long.Parse);

                        using (var con = new SqlConnection(Config.DBConnectionString))
                        {
                            mandatorySkuMappings = con.Query<SaudaConditionalBookingMandatorySkuPricingDto>("[dbo].[usp_GetMandatorySkuMappingList]",
                               new
                               {
                                   EssentialSkuMappingId = config.Id,
                                   PlantId = plantId,
                                   CurrentDate = currentDate
                               },
                               commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 0).ToList();
                        }

                        bool allEssentialSkusPresent = essentialSkuIds.All(id => selectedSkuIds.Contains(id));

                        if (allEssentialSkusPresent && mandatorySkuMappings.Any())
                        {
                            var selectedMandatorySkuIds = mandatorySkuMappings.Where(_ => selectedSkuIds.Contains(_.MandatorySkuId))
                                .Select(_ => _.MandatorySkuId).ToList();

                            if (!selectedMandatorySkuIds.Any())
                            {
                                isBookingValid = false;
                                errorMessage = Constants.MandatorySkusNotFoundOnBooking;
                                continue;
                            }

                            bool mandatoryMatch = mandatorySkuMappings
                                .Select(x => x.MandatorySkuId)
                                .OrderBy(x => x)
                                .SequenceEqual(selectedMandatorySkuIds.OrderBy(x => x));

                            if (!mandatoryMatch)
                            {
                                isBookingValid = false;
                                errorMessage = Constants.MandatorySkusNotMatch;
                                continue;
                            }

                            var totalEssentialQuantity = inputDto.SaudaOrders
                                .Where(o => essentialSkuIds.Contains(o.SkuId))
                                .Sum(o => o.BidQuantity);

                            foreach (var mandatorySku in mandatorySkuMappings)
                            {
                                var mandatoryQuantity = inputDto.SaudaOrders
                                    .FirstOrDefault(o => o.SkuId == mandatorySku.MandatorySkuId)?.BidQuantity ?? 0;

                                if (mandatoryQuantity == 0)
                                {
                                    errorMessage = string.Format(Constants.MandatoryQuantityNotExits, mandatorySku.MandatorySkuName);
                                    isConditionValid = false;
                                    isBookingValid = false;
                                    break;
                                }

                                var bookingPercentage = (mandatoryQuantity / totalEssentialQuantity) * 100;

                                if (bookingPercentage < mandatorySku.MandatoryBookingQuantityPercentage)
                                {
                                    errorMessage = string.Format(Constants.MandatoryQuantityNotMatch, mandatorySku.MandatorySkuName, mandatorySku.MandatoryBookingQuantityPercentage);
                                    isConditionValid = false;
                                    isBookingValid = false;
                                    break;
                                }

                                isConditionValid = true;

                                foreach (var order in inputDto.SaudaOrders)
                                {
                                    if (order.SkuId == mandatorySku.MandatorySkuId)
                                    {
                                        order.IsMandatorySku = true;
                                    }
                                    else
                                    {
                                        if (!selectedMandatorySkuIds.Contains(order.SkuId))
                                        {
                                            order.IsMandatorySku = false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            isBookingValid = true;
                        }
                    }
                }
                else
                {
                    isBookingValid = true;
                }

                if (isConditionValid)
                {
                    isBookingValid = true;
                }


                return new Tuple<bool, string>(isBookingValid, errorMessage);
            }
            catch (Exception ex)
            {
                _logger.Info($"Service {ServiceName} Method {methodName} Exception: {ex.Message}");
                return new Tuple<bool, string>(isBookingValid, errorMessage);
            }
        }

        public long GetDiscountId(SaudaInputDto inputDto, SaudaOrderInputDto orderInputDto)
        {
            long result = 0;
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            var methodName = "GetDiscountId";

            try
            {     
                var userStateId = _adaniContext.Users.AsNoTracking().Where(u => u.Id == inputDto.DealerId)
                        .Select(u => u.StateId).FirstOrDefault();

                if (userStateId == 0)
                    return result;

                if (orderInputDto.DiscountTypeId == (long)DTO.Enums.SaudaDiscountType.Discount)
                {
                    var discountData = _adaniContext.DiscountGeography
                        .AsNoTracking()
                        .FirstOrDefault(d =>
                            currentDate >= d.ValidFrom &&
                            currentDate <= d.ValidTo &&
                            (d.StateId == userStateId || d.StateId == 0) &&
                            d.SkuId == orderInputDto.SkuId);

                    if (discountData != null)
                        result = discountData.Id;
                }
                else
                {
                    var premiumData = _adaniContext.PremiumUser
                        .AsNoTracking()
                        .FirstOrDefault(p =>
                            p.ParentId != 0 &&
                            DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(p.ValidFrom) &&
                            DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(p.ValidTo) &&
                            p.UserId == inputDto.LoginUserId &&
                            p.SkuId == orderInputDto.SkuId);

                    if (premiumData != null)
                        result = premiumData.Id;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Info($"Service {ServiceName} Method {methodName} Exception: {ex.Message}");
                return result;
            }
        }

        public decimal GetSaudaBookedQuantityForCurrentDate(SaudaInputDto inputDto, long oilTypeId)
        {
            var overallSaudaStatuses = Constants.OverallSaudaStatus;
            var currentDate = DateTime.Today;
            decimal saudaBidQuantity = 0;

            try
            {
                saudaBidQuantity = _adaniContext.SaudaOrders
                    .AsNoTracking()
                    .Where(_ => _.Sauda != null
                        && _.OilTypeId == oilTypeId
                        && inputDto.DealerId == _.Sauda.UserId
                        && DbFunctions.TruncateTime(_.Sauda.BiddingDate) == DbFunctions.TruncateTime(currentDate)
                        && !_.IsReportingtoAllocation
                        && overallSaudaStatuses.Contains(_.StatusId)
                        && _.IsQuantityLimitForBookingSauda
                        && _.SalesOrganizationId == inputDto.SalesOrganizationId
                        && _.DistributionChannelId == inputDto.DistributionChannelId
                        && _.DivisionId == inputDto.DivisionId)
                    .Select(s => s.BidQuantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                return saudaBidQuantity;
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Service-Method {nameof(GetSaudaBookedQuantityForCurrentDate)} Exception: {exception}");
                return saudaBidQuantity;
            }
        }

        public decimal GetSaudaBookedQuantityForCurrentDateByDealers(SaudaInputDto inputDto, long oilTypeId, List<long> DealerList)
        {
            var overallSaudaStatuses = Constants.OverallSaudaStatus;
            var currentDate = DateTime.Now.Date;
            decimal saudaBidQuantity = 0;

            try
            {
                saudaBidQuantity = _adaniContext.SaudaOrders
                    .AsNoTracking()
                    .Where(_ => _.Sauda != null
                        && _.OilTypeId == oilTypeId
                        && DealerList.Contains(_.Sauda.UserId)
                        && DbFunctions.TruncateTime(_.Sauda.BiddingDate) == DbFunctions.TruncateTime(currentDate)
                        && !_.IsReportingtoAllocation
                        && overallSaudaStatuses.Contains(_.StatusId)
                        && _.IsQuantityLimitForBookingSauda
                        && _.SalesOrganizationId == inputDto.SalesOrganizationId
                        && _.DistributionChannelId == inputDto.DistributionChannelId
                        && _.DivisionId == inputDto.DivisionId)
                    .Select(s => s.BidQuantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                return saudaBidQuantity;
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Service-Method {nameof(GetSaudaBookedQuantityForCurrentDate)} Exception: {exception}");
                return saudaBidQuantity;
            }
        }

        public bool IsSalesAreaBookingValid(SaudaInputDto inputDto)
        {
            var methodName = "IsSalesAreaBookingValid";
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

            try
            {
                var restriction = _adaniContext.SaudaSalesAreaRestrictions
                .Where(x => x.SalesOrganizationId == inputDto.SalesOrganizationId
                         && x.DistributionChannelId == inputDto.DistributionChannelId
                         && x.DivisionId == inputDto.DivisionId
                         && x.IsActive
                         && DbFunctions.TruncateTime(x.ValidFrom) <= currentDate.Date
                         && DbFunctions.TruncateTime(x.ValidTo) >= currentDate.Date)
                .FirstOrDefault();

                if (restriction == null)
                    return true;

                DateTime restrictionTimeToday = currentDate.Date
                    .Add(TimeSpan.Parse(restriction.TimeRestriction.ToString()));

                if (currentDate >= restrictionTimeToday)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Info($"Service {ServiceName} Method {methodName} Exception: {ex.Message}");
                return false;
            }
        }

        public decimal CalculateAutomatedDiscount(decimal discountAmount, long convertFromSkuId, long convertToSkuId)
        {
            var calculatedDiscount = (decimal)0;
            var methodName = "CalculateAutomatedDiscount";

            try
            {
                if (convertFromSkuId == convertToSkuId)
                    return discountAmount;

                var skus = _adaniContext.Skus
                .AsNoTracking()
                .Where(s => s.Id == convertFromSkuId || s.Id == convertToSkuId)
                .Select(s => new
                {
                    s.Id,
                    s.DiscountAutomationConversionFactor2
                })
            .ToList();

                var fromFactor = skus
                    .FirstOrDefault(s => s.Id == convertFromSkuId)
                    ?.DiscountAutomationConversionFactor2;

                var toFactor = skus
                    .FirstOrDefault(s => s.Id == convertToSkuId)
                    ?.DiscountAutomationConversionFactor2;

                // If SKU not found or factors invalid → return 0
                if (fromFactor.GetValueOrDefault() <= 0 || toFactor.GetValueOrDefault() <= 0)
                    return calculatedDiscount;

                // No conversion needed
                if (fromFactor == toFactor)
                    return discountAmount;

                return (discountAmount / fromFactor.Value) * toFactor.Value;
            }
            catch (Exception ex)
            {
                _logger.Info($"Service {ServiceName} Method {methodName} Exception: {ex.Message}");
                return calculatedDiscount;
            }
        }

        public decimal GetSaudaBookedQuantityForCurrentDateByDealersByDateRangeIsReportingtoAllocation(
    SaudaInputDto inputDto,
    long oilTypeId,
    List<long> dealerList,
    DateTime validFrom,
    DateTime validTo)
        {
            var overallSaudaStatuses = Constants.OverallSaudaStatus;
            decimal saudaBidQuantity = 0;

            try
            {
                var todayStart = DateTime.Now.Date;
                var todayEnd = todayStart.AddDays(1).AddTicks(-1);

                var effectiveStart = validFrom > todayStart ? validFrom : todayStart;
                var effectiveEnd = validTo < todayEnd ? validTo : todayEnd;

                // If today is outside valid window → return 0
                if (effectiveStart > effectiveEnd)
                    return 0;

                saudaBidQuantity = _adaniContext.SaudaOrders
                    .AsNoTracking()
                    .Where(_ => _.Sauda != null
                        && _.OilTypeId == oilTypeId
                        && dealerList.Contains(_.Sauda.UserId)
                        && _.Sauda.BiddingDate >= effectiveStart
                        && _.Sauda.BiddingDate <= effectiveEnd
                        && _.IsReportingtoAllocation
                        && overallSaudaStatuses.Contains(_.StatusId)
                        && _.IsQuantityLimitForBookingSauda
                        && _.SalesOrganizationId == inputDto.SalesOrganizationId
                        && _.DistributionChannelId == inputDto.DistributionChannelId
                        && _.DivisionId == inputDto.DivisionId)
                    .Select(s => s.BidQuantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                return saudaBidQuantity;
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Service-Method {nameof(GetSaudaBookedQuantityForCurrentDateByDealersByDateRangeIsReportingtoAllocation)} Exception: {exception}");
                return saudaBidQuantity;
            }
        }

        public decimal GetSaudaBookedQuantityForCurrentDateByDealersByDateRangeIsNotReportingtoAllocation(
    SaudaInputDto inputDto,
    long oilTypeId,
    List<long> dealerList,
    DateTime validFrom,
    DateTime validTo)
        {
            var overallSaudaStatuses = Constants.OverallSaudaStatus;
            decimal saudaBidQuantity = 0;

            try
            {
                var todayStart = DateTime.Now.Date;
                var todayEnd = todayStart.AddDays(1).AddTicks(-1);

                var effectiveStart = validFrom > todayStart ? validFrom : todayStart;
                var effectiveEnd = validTo < todayEnd ? validTo : todayEnd;

                // If today is outside valid window → return 0
                if (effectiveStart > effectiveEnd)
                    return 0;

                saudaBidQuantity = _adaniContext.SaudaOrders
                    .AsNoTracking()
                    .Where(_ => _.Sauda != null
                        && _.OilTypeId == oilTypeId
                        && dealerList.Contains(_.Sauda.UserId)
                        && _.Sauda.BiddingDate >= effectiveStart
                        && _.Sauda.BiddingDate <= effectiveEnd
                        && !_.IsReportingtoAllocation
                        && overallSaudaStatuses.Contains(_.StatusId)
                        && _.IsQuantityLimitForBookingSauda
                        && _.SalesOrganizationId == inputDto.SalesOrganizationId
                        && _.DistributionChannelId == inputDto.DistributionChannelId
                        && _.DivisionId == inputDto.DivisionId)
                    .Select(s => s.BidQuantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                return saudaBidQuantity;
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Service-Method {nameof(GetSaudaBookedQuantityForCurrentDateByDealersByDateRangeIsNotReportingtoAllocation)} Exception: {exception}");
                return saudaBidQuantity;
            }
        }

        public decimal GetSaudaBookedQuantityForCurrentDateByDateRangeIsNotReportingtoAllocation(
    SaudaInputDto inputDto,
    long oilTypeId,
    DateTime validFrom,
    DateTime validTo)
        {
            var overallSaudaStatuses = Constants.OverallSaudaStatus;
            decimal saudaBidQuantity = 0;

            try
            {
                var todayStart = DateTime.Now.Date;
                var todayEnd = todayStart.AddDays(1).AddTicks(-1);

                var effectiveStart = validFrom > todayStart ? validFrom : todayStart;
                var effectiveEnd = validTo < todayEnd ? validTo : todayEnd;

                // If today is outside valid window → no data
                if (effectiveStart > effectiveEnd)
                    return 0;

                saudaBidQuantity = _adaniContext.SaudaOrders
                    .AsNoTracking()
                    .Where(_ => _.Sauda != null
                        && _.OilTypeId == oilTypeId
                        && inputDto.DealerId == _.Sauda.UserId
                        && _.Sauda.BiddingDate >= effectiveStart
                        && _.Sauda.BiddingDate <= effectiveEnd
                        && !_.IsReportingtoAllocation
                        && overallSaudaStatuses.Contains(_.StatusId)
                        && _.IsQuantityLimitForBookingSauda
                        && _.SalesOrganizationId == inputDto.SalesOrganizationId
                        && _.DistributionChannelId == inputDto.DistributionChannelId
                        && _.DivisionId == inputDto.DivisionId)
                    .Select(s => s.BidQuantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                return saudaBidQuantity;
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Service-Method {nameof(GetSaudaBookedQuantityForCurrentDateByDateRangeIsNotReportingtoAllocation)} Exception: {exception}");
                return saudaBidQuantity;
            }
        }

        public decimal GetSaudaBookedQuantityForCurrentDateByDateRangeIsReportingtoAllocation(
    SaudaInputDto inputDto,
    long oilTypeId,
    DateTime validFrom,
    DateTime validTo)
        {
            var overallSaudaStatuses = Constants.OverallSaudaStatus;
            decimal saudaBidQuantity = 0;

            try
            {
                var todayStart = DateTime.Now.Date;
                var todayEnd = todayStart.AddDays(1).AddTicks(-1);

                var effectiveStart = validFrom > todayStart ? validFrom : todayStart;
                var effectiveEnd = validTo < todayEnd ? validTo : todayEnd;

                // If today is outside valid window → no data
                if (effectiveStart > effectiveEnd)
                    return 0;

                saudaBidQuantity = _adaniContext.SaudaOrders
                    .AsNoTracking()
                    .Where(_ => _.Sauda != null
                        && _.OilTypeId == oilTypeId
                        && inputDto.DealerId == _.Sauda.UserId
                        && _.Sauda.BiddingDate >= effectiveStart
                        && _.Sauda.BiddingDate <= effectiveEnd
                        && _.IsReportingtoAllocation
                        && overallSaudaStatuses.Contains(_.StatusId)
                        && _.IsQuantityLimitForBookingSauda
                        && _.SalesOrganizationId == inputDto.SalesOrganizationId
                        && _.DistributionChannelId == inputDto.DistributionChannelId
                        && _.DivisionId == inputDto.DivisionId)
                    .Select(s => s.BidQuantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                return saudaBidQuantity;
            }
            catch (Exception exception)
            {
                _logger.Error($"{ServiceName} Service-Method {nameof(GetSaudaBookedQuantityForCurrentDateByDateRangeIsReportingtoAllocation)} Exception: {exception}");
                return saudaBidQuantity;
            }
        }
    }
}
