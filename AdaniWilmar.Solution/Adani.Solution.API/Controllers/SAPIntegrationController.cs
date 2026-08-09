using Adani.Solution.API.Infrastructure;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using GMCore.Authenticate;
using GMCore.Helper;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Services.Description;

namespace Adani.Solution.API.Controllers
{
    [SAPBasicAuthenticationFilter]
    [CustomException]
    [RoutePrefix("api/sap")]
    public class SAPIntegrationController : BaseApiController
    {
        private const string ServiceName = "SAPIntegration Controller";
        private readonly ISAPIntegrationService _sapIntegrationService;
        private string _methodName;

        public SAPIntegrationController(ISAPIntegrationService sapIntegrationService) : base(ServiceName)
        {
            _methodName = "Lookup Controller";
            try
            {
                _sapIntegrationService = sapIntegrationService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }
        #region Old Interface

        #region StateCityDistrict
        [HttpPost]
        [Route("statecitydistrict/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveStateCityDistrict", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveStateCityDistrict([FromBody] string inputKey)
        {
            _methodName = "SaveStateCityDistrict";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _sapIntegrationService.SaveStateCityDistrict(decryptedInput);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(result.ErrorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        #endregion

        #region Customer/Broker
        [HttpPost]
        [Route("user/create")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveCustomer", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveCustomer(HANASAPCustomerDtoList inputdto)
        {
            _methodName = "SaveCustomer";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaveCustomer(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);

        }
        #endregion

        #region TradeTicket
        /// <summary>
        /// Method to Get trade ticket details List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("tradeticket/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetTradeTicketDetails()
        {
            _methodName = "GetTradeTicketDetails";
            var result = new ResultDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.GetTradeTicketDetails();
            });

            successDto.Response = result.SuccessDto.Response;
            contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
            return Ok(contentDto);
        }

        /// <summary>
        /// Method to Get trade ticket details List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("tradeticketSF/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSpecialityFatTradeTicketDetails()
        {
            _methodName = "GetSpecialityFatTradeTicketDetails";
            var result = new ResultDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.GetSpecialityFatTradeTicketDetails();
            });

            successDto.Response = result.SuccessDto.Response;
            contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
            return Ok(contentDto);
        }


        [HttpPost]
        [Route("tradeticket/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateTradeTicketNumber", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateTradeTicketNumber([FromBody] string inputKey)
        {
            _methodName = "UpdateTradeTicketNumber";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.UpdateTradeTicketNumber(decryptedInput);
                });


                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(result.ErrorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.Response = result.ErrorDto.Response;
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }

        }

        [HttpPost]
        [Route("tradeticket/create")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "CreateTradeTicketNumber", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult CreateTradeTicketNumber(TradeTicketListDto inputdto)
        {
            _methodName = "CreateTradeTicketNumber";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.CreateTradeTicket(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("tradeticketsf/create")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "CreateTradeTicketSF", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult CreateTradeTicketSF([FromBody] string inputKey)
        {
            _methodName = "CreateTradeTicketSF";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.CreateTradeTicketSF(decryptedInput);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(result.ErrorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.Response = result.ErrorDto.Response;
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }

        }
        #endregion       

        #region SaudaApproval
        /// <summary>
        /// Method to get sauda approval details List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("sauda/approval/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaApprovalDetails()
        {
            _methodName = "GetSaudaApprovalDetails";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                //_sapIntegrationService.GetSaudaApprovalDetails();
            });
            return Ok(result);
        }

        //[HttpPost]
        //[Route("saudaapproval/confirmation")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaApprovalConfirmation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaudaApprovalConfirmation(HANASaudaApprovalConfirmationDtoList inputdto)
        //{
        //    _methodName = "SaudaApprovalConfirmation";
        //    var result = new ResultDto();
        //    try
        //    {
        //        _logger.Info($"{ServiceName} Controller-Method {_methodName}");

        //        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
        //        {
        //            _sapIntegrationService.SaudaApprovalConfirmation(inputdto);
        //        });
        //        result.IsSuccess = true;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        result.IsSuccess = false;
        //        result.ErrorDto.ErrorCode = Constants.Exception;
        //        result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
        //    }
        //    return Ok(result);

        //}

        #endregion

        #region Sauda Amendment
        [HttpPost]
        [Route("sauda/amendment")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaAmendment", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaAmendment(HANASaudaAmendmentDtoList inputdto)
        {
            _methodName = "SaudaAmendment";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaudaAmendment(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);

        }
        #endregion      

        #region SaudaLimit
        /// <summary>
        /// Method to Get Sauda Limit List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("saudalimit/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaLimitDetails()
        {
            _methodName = "GetLiftingRequestDetails";
            var result = new ResultDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.GetSaudaLimitDetails();
            });
            successDto.Response = result.SuccessDto.Response;
            contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
            return Ok(contentDto);
        }

        /// <summary>
        /// Method to Save Sauda Limit 
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saudalimit/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveSaudaLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveSaudaLimit(HANASaudaLimitDto inputdto)
        {
            _methodName = "SaveSaudaLimit";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    // _sapIntegrationService.SaveSaudaLimit(inputdto);
                });

                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }
        #endregion

        #region Sku     

        [HttpPost]
        [Route("sku/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveSku", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveSku([FromBody] string inputKey)
        {
            _methodName = "SaveSku";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    //_sapIntegrationService.SaveSku(decryptedInput);
                });

                result.IsSuccess = true;

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(result.ErrorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.Response = result.ErrorDto.Response;
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }

        }
        #endregion

        #region CreditMaster     

        [HttpPost]
        [Route("credit/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveCreditMaster", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveCreditMaster(HANACreditMasterDtoList inputdto)
        {
            _methodName = "SaveCreditMaster";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaveCreditMaster(inputdto);
                });

                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }
        #endregion

        #region Depot     

        [HttpPost]
        [Route("depot/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveDepot", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveDepot([FromBody] string inputKey)
        {
            _methodName = "SaveDepot";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaveDepot(decryptedInput);
                });

                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(result.ErrorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.Response = result.ErrorDto.Response;
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }

        }
        #endregion        

        #region DO Update and Delete
        [HttpPost]
        [Route("do/delete")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DeleteDO", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DeleteDO([FromBody] string inputKey)
        {
            _methodName = "DeleteDO";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.DODelete(decryptedInput);
                });

                result.IsSuccess = true;

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(result.ErrorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.Response = result.ErrorDto.Response;
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }

        }

        [HttpPost]
        [Route("do/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateDO", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateDO([FromBody] string inputKey)
        {
            _methodName = "UpdateDO";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.DOUpdate(decryptedInput);
                });

                result.IsSuccess = true;

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(result.ErrorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.Response = result.ErrorDto.Response;
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }

        }
        #endregion                

        #region PendingContract 
        [HttpPost]
        [Route("PendingContractReport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PendingContractReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PendingContractReport(PendingContractListDto inputdto)
        {
            _methodName = "PendingContractReport";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    // _sapIntegrationService.PendingContractReport(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }



        //[HttpPost]
        //[Route("TruckPlacementTrackerReport")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "TruckPlacementTrackerReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult TruckPlacementTrackerReport(TruckPlacementTrackerList inputdto)
        //{
        //    _methodName = "TruckPlacementTrackerReport";
        //    var result = new ResultDto();
        //    try
        //    {
        //        _logger.Info($"{ServiceName} Controller-Method {_methodName}");

        //        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
        //        {
        //            _sapIntegrationService.TruckPlacementTrackerReport(inputdto);
        //        });
        //        result.IsSuccess = true;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        result.IsSuccess = false;
        //        result.ErrorDto.ErrorCode = Constants.Exception;
        //        result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
        //    }
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("chequeinventoryreport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ChequeInventoryReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ChequeInventoryReport(HANAChequeStatusDtoList inputdto)
        {
            _methodName = "ChequeInventoryReport";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.ChequeInventoryReport(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }
        #endregion

        #region Sauda Conversion & Extension 
        [HttpPost]
        [Route("sauda/conversion/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaConversionNumberUpdate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaConversionNumberUpdate(HANASaudaConversionDtoList inputdto)
        {
            _methodName = "SaudaConversionNumberUpdate";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaudaConversionNumberUpdate(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }

        /// <summary>
        /// Method to Get Lifting Request List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getsauda/conversion")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaConversionOutboundDetails()
        {
            _methodName = "GetSaudaConversionOutboundDetails";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                //_sapIntegrationService.GetSaudaConversionDetails();
            });
            return Ok(result);
        }

        //[HttpPost]
        //[Route("sauda/extension/update")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaExtensionUpdate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaudaExtensionUpdate(HANASaudaExtension inputdto)
        //{
        //    _methodName = "SaudaExtensionUpdate";
        //    var result = new ResultDto();
        //    try
        //    {
        //        _logger.Info($"{ServiceName} Controller-Method {_methodName}");

        //        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
        //        {
        //            _sapIntegrationService.SaudaExtensionUpdate(inputdto);
        //        });
        //        result.IsSuccess = true;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        result.IsSuccess = false;
        //        result.ErrorDto.ErrorCode = Constants.Exception;
        //        result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
        //    }
        //    return Ok(result);

        //}

        /// <summary>
        /// Method to Get Lifting Request List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getsauda/extension")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaextensionOutboundDetails()
        {
            _methodName = "GetSaudaExtensionOutboundDetails";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                //_sapIntegrationService.GetSaudaExtensionDetails();
            });
            return Ok(result);
        }
        #endregion

        #region  Save SKU Details

        [HttpPost]
        [Route("skudetails/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveSkuDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveSkuDetails(HANASAPSku inputdto)
        {
            _methodName = "SaveSkuDetails";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaveSkuDetails(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }


        #endregion

        #region Sauda Release 

        [HttpPost]
        [Route("sauda/release/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaReleaseUpdate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaReleaseUpdate(SaudaReleaseSAPToAPPDto inputdto)
        {
            _methodName = "SaudaReleaseUpdate";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaudaReleaseSAPToAPP(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);

        }

        #endregion

        #region PendingContractSync
        /// <summary>
        /// Method to Get Sauda details List - Trigger
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("sauda/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetPendingContractSync()
        {
            _methodName = "GetPendingContractSync";
            var result = new ResultDto();
            //var verticalId = 0;
            //var tradeTicketWithOrWithoutId = 0;
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                //_sapIntegrationService.GetSaudaDetails(saudaIds);
                //_sapIntegrationService.GetSaudaDetails(verticalId, tradeTicketWithOrWithoutId);
            });
            return Ok(result);
        }
        #endregion

        #endregion

        #region AWL Interface 

        #region Sauda
        /// <summary>
        /// Method to Get Sauda details List - Trigger
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("sauda/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaDetails(List<long> saudaIds = null)
        {
            _methodName = "GetSaudaDetails";
            var result = new ResultDto();
            //var verticalId = 0;
            //var tradeTicketWithOrWithoutId = 0;
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
               // _sapIntegrationService.GetSaudaDetails(saudaIds, false);
                //_sapIntegrationService.GetSaudaDetails(verticalId, tradeTicketWithOrWithoutId);
            });
            return Ok(result);
        }

        /// <summary>
        /// Method to Get Sauda details List - Trigger
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("sauda/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSaudaDetails(SAPDataSyncInputDto inputDto)
        {
            _methodName = "GetSaudaDetails";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                //_sapIntegrationService.GetSaudaDetails();
                // _sapIntegrationService.GetSaudaDetails(inputDto.VerticalId, inputDto.TradeTicketWithOrWithoutId);
            });
            return Ok(result);
        }


        [HttpPost]
        [Route("sauda/create")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "CreateSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult CreateSauda(SaudaCreateSAPToAPPDto inputdto)
        {
            _methodName = "CreateSauda";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaudaCreate(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);

        }


        #endregion

        #region Pricing Details SAP To APP 

        [HttpPost]
        [Route("pricing")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SavePricingDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SavePricingDetails(HANAPricing inputdto)
        {
            _methodName = "SavePricingDetails";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SavePricingDetails(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);

        }

        #endregion

        #region OpenContract and OpenBalance

        [HttpPost]
        [Route("pendingcontractautotrigger")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult PendingContractAutoTrigger()
        {
            _methodName = "PendingContractAutoTrigger";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.PendingContractAutoTrigger();
                // _sapIntegrationService.GetSaudaDetails(inputDto.VerticalId, inputDto.TradeTicketWithOrWithoutId);
            });
            return Ok(result);
        }

        [HttpPost]
        [Route("contractopenbalancerequest")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult ContractOpenBalanceRequest(OpenContractRequestDTOList inputDto)
        {
            _methodName = "ContractOpenBalanceRequest";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.ContractTrigger(inputDto);
                // _sapIntegrationService.GetSaudaDetails(inputDto.VerticalId, inputDto.TradeTicketWithOrWithoutId);
            });
            return Ok(result);
        }


        [HttpPost]
        [Route("contractopenbalanceresponse")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult ContractOpenBalanceResponce(HANAOpenBalAndOpenContractDTOList inputdto)
        {
            _methodName = "ContractOpenBalanceResponce";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.ContractOpenBalanceResponce(inputdto);
                // _sapIntegrationService.GetSaudaDetails(inputDto.VerticalId, inputDto.TradeTicketWithOrWithoutId);
            });
            return Ok(result);
        }

        #endregion

        #region Sauda Limit AWL   

        [HttpPost]
        [Route("saudalimitresponse")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult SaudaLimitResponce(HANASaudaLimitList inputDto)
        {
            _methodName = "SaudaLimitResponce";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.SaudaLimitResponce(inputDto);
                // _sapIntegrationService.GetSaudaDetails(inputDto.VerticalId, inputDto.TradeTicketWithOrWithoutId);
            });
            return Ok(result);
        }
        #endregion

        #region Responce API SAP To APP 
        [HttpPost]
        [Route("sauda/commonfunction")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaCommonFunction", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaCommonFunction(HANASaudaCommonFunctionList inputdto)
        {
            string flag = inputdto.Flag;
            _methodName = "SaudaCommonFunction";

            var input = new List<HANASaudaCommonFunctionList>();
            input.Add(inputdto);

            var result = new ResultDto();
            try
            {
                //if (flag == ((int)DTO.Enums.SaudaFunctionTypes.SaudaExtensionUpdate).ToString())
                //{
                //    //HANASaudaExtension
                //    _methodName += " - SaudaExtensionUpdate";
                //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                //    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                //    {
                //        _sapIntegrationService.SaudaExtensionUpdate(input);
                //    });
                //    result.IsSuccess = true;
                //}
                //else
                if (flag == ((int)DTO.Enums.SaudaFunctionTypes.SalesOrder).ToString())
                {
                    //HANALiftingRequestInquiryNumberDtoList
                    _methodName += " - LiftingRequestEnquiryNumberUpdate";
                    _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        _sapIntegrationService.LiftingRequestEnquiryNumberUpdate(input);
                    });
                    result.IsSuccess = true;

                }
                else if (flag == ((int)DTO.Enums.SaudaFunctionTypes.SalesOrderDeliveryNoUpdate).ToString())
                {
                    //HANALiftingRequestInquiryNumberDtoList
                    _methodName += " - LiftingRequestEnquiryNumberUpdate";
                    _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        _sapIntegrationService.LiftingRequestDeliveryNoUpdate(input);
                    });
                    result.IsSuccess = true;

                }
                else if (flag == ((int)DTO.Enums.SaudaFunctionTypes.SalesOrderInvoicNoUpdate).ToString())
                {
                    //HANALiftingRequestInquiryNumberDtoList
                    _methodName += " - LiftingRequestEnquiryNumberUpdate";
                    _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        _sapIntegrationService.LiftingRequestInvoicNoUpdate(input);
                    });
                    result.IsSuccess = true;

                }
                else if (flag == ((int)DTO.Enums.SaudaFunctionTypes.SaudaNumberUpdate).ToString())
                {
                    //SaudaNumberUpdate
                    _methodName += " - SaudaNumberUpdate";
                    _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        _sapIntegrationService.UpdateSaudaNumber(input);
                    });
                    result.IsSuccess = true;

                }
                else if (flag == ((int)DTO.Enums.SaudaFunctionTypes.SaudaChange).ToString())
                {
                    //SaudaNumberUpdate
                    _methodName += " - SaudaChange";
                    _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        _sapIntegrationService.UpdateSaudaChange(input);
                    });
                    result.IsSuccess = true;

                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }

        #endregion

        #region Sales Order


        /// <summary>
        /// Method to Get Lifting Request List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getliftingrequest/enquirynumber")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetLiftingRequestEnquiryNumberOutboundDetails()
        {
            _methodName = "GetLiftingRequestEnquiryNumberOutboundDetails";
            _methodName = "GetLiftingRequestDetails";
            var result = new ResultDto();
            List<long> liftingRequestId = new List<long>();
            liftingRequestId.Add(100);
            bool IsReprocess = false;
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.GetLiftingRequestEnquiryNumberOutboundDetails(liftingRequestId, IsReprocess);
            });
            return Ok(result);
        }

        [HttpPost]
        [Route("liftingrequest/createsaptoapp")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "LiftRequestCreateSapToApp", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult LiftRequestCreateSapToApp(SalesOrderCreate inputdto)
        {
            _methodName = "LiftRequestCreateSapToApp";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.LiftRequestCreateSapToApp(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);

        }

        #endregion

        #region Invoice
        [HttpPost]
        [Route("invoice")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveInvoice", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveInvoice(InvoiceDto inputdto)
        {
            _methodName = "SaveInvoice";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaveInvoice(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("invoice/paymentstatus/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateInvoicePaymentStatus", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateInvoicePaymentStatus([FromBody] string inputKey)
        {
            _methodName = "UpdateInvoicePaymentStatus";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.InvoiceStatusChange(decryptedInput);
                });

                result.IsSuccess = true;

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(result.ErrorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.Response = result.ErrorDto.Response;
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        #endregion        

        #region CustomerLedger

        [HttpPost]
        [Route("customerledgerautotrigger")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult CustomerLedgerautotrigger()
        {
            _methodName = "CustomerLedgerautotrigger";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.CustomerLedgerAutoTrigger();
            });
            return Ok(result);
        }

        [HttpPost]
        [Route("customerledger/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveCustomerLedger", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveCustomerLedger(HANACustomerLedgerDtoList inputdto)
        {
            _methodName = "SaveCustomerLedger";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaveCustomerLedger(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("overduepayment/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveOverduePayment", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveOverduePayment(HANACustomerLedgerDtoList inputdto)
        {
            _methodName = "SaveOverduePayment";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SaveOverduePayment(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }
        #endregion

        #region Sales Report

        [HttpPost]
        [Route("SalesReport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SalesReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SalesReport(AWLSalesRegisterOutputDto inputdto)
        {
            _methodName = "SalesReport";
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.SalesReport(inputdto);
                });
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }




        #endregion

        #region Darwinbox

        [HttpPost]
        [Route("employeerequestactiveusers")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult EmployeeRequestActiveUsers()
        {
            _methodName = "EmployeeRequestActiveUsers";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.EmployeeRequestActiveUsers();
            });
            return Ok(result);
        }

        [HttpPost]
        [Route("employeerequestinactiveusers")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult EmployeeRequestInActiveUsers()
        {
            _methodName = "EmployeeRequestInActiveUsers";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.EmployeeRequestInActiveUsers();
            });
            return Ok(result);
        }
        #endregion

        #region OverDue 
        [HttpPost]
        [Route("saudasxpirednotification")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult SaudaExpiredNotification()
        {
            _methodName = "SaudaExpiredNotification";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.SaudaExpiredNotification();
            });
            return Ok(result);
        }

        [HttpPost]
        [Route("overduenotification")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult OverDueNotification()
        {
            _methodName = "OverDueNotification";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.OverDueNotification();
            });
            return Ok(result);
        }
        #endregion

        #endregion

        #region Call Recording
        [HttpPost]
        [Route("saveCallRecordingOfCustomers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveCallRecordingOfCustomers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveCallRecordingOfCustomers([FromUri] long key)
        {
            _methodName = "SaveCallRecordingOfCustomers";
            _logger.Info($"SAP Controller : {ServiceName} Controller-Method {_methodName}");
            var file = HttpContext.Current.Request.Files[0];
            var pageId = (int)DTO.Enums.PageType.AudioFiles;
            var inputDto = new CallRecordingInputDto();
            var dailerId = HttpContext.Current.Request.Form.Get("DialerId");
            var receiverId = HttpContext.Current.Request.Form.Get("ReceiverId");
            inputDto.DialerMobileNumber = HttpContext.Current.Request.Form.Get("DialerMobileNumber");
            inputDto.ReceiverMobileNumber = HttpContext.Current.Request.Form.Get("ReceiverMobileNumber");
            var callDuration = HttpContext.Current.Request.Form.Get("CallDuation");
            var callStartTime = HttpContext.Current.Request.Form.Get("CallStartTime");

            inputDto.DialerId = Convert.ToInt64(dailerId);
            inputDto.ReceiverId = Convert.ToInt64(receiverId);
            inputDto.CallDuation = Convert.ToInt32(callDuration);
            inputDto.CallStartTime = callStartTime;

            var result = new ResultDto();
            try
            {
                result = _sapIntegrationService.SaveCallRecordingOfCustomers(inputDto, file, file.FileName, pageId);
                result.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("dialermobilenumber/bybdodetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DialerMobileNumberByBDODetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DialerMobileNumberByBDODetails(string DialerMobileNumber = "", long VerticalId = 0, long DealerId = 0)
        {
            _methodName = "DialerMobileNumberByBDODetails";
            var inputDto = new CallRecordingGetInputDto { DialerMobileNumber = DialerMobileNumber, DealerId = DealerId, VerticalId = VerticalId };
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _sapIntegrationService.DialerMobileNumberByBDODetails(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("bdo/dealerdetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerDetailsByBDO", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerDetailsByBDO(string DialerMobileNumber = "")
        {
            _methodName = "GetDealerDetailsByBDO";
            var inputDto = new CallRecordingGetInputDto { DialerMobileNumber = DialerMobileNumber };
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _sapIntegrationService.GetDealerDetailsByBDO(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("dealer/bdodetailswithmasterdata")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerDetailsByBDO", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDODetailsWithMasterData(string DialerMobileNumber = "")
        {
            _methodName = "GetBDODetailsWithMasterData";
            var inputDto = new CallRecordingGetInputDto { DialerMobileNumber = DialerMobileNumber };
            var result = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _sapIntegrationService.GetBDODetailsWithMasterData(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            return Ok(result);
        }
        #endregion

        #region  Account Statement
        [HttpPost]
        [Route("sapaccountstatement")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult AccountStatement(List<SAPAccountStatementDto> inputDto)
        {
            _methodName = "AccountStatement";
            var result = new ResultDto();
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
            {
                _sapIntegrationService.AccountStatement(inputDto);
            });
            return Ok(result);
        }
        #endregion
    }
}