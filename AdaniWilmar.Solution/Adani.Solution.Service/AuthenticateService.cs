using GMCore.Authenticate;
using GMCore.Helper;
using GMCore.Logger;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Adani.Solution.Service
{
    public interface IAuthenticateService
    {
        ResultDto ValidateAppKey(KeyInputDto keyInputDto);
        ResultDto VerifyWebKey(KeyInputDto keyInputDto);
    }

    public class AuthenticateService : IAuthenticateService
    {
        private readonly IAdaniContext _emamiContext;
        private const string ServiceName = "Authenticate Service";
        private string _methodName;
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);

        public AuthenticateService(IAdaniContext emamiContext)
        {
            try
            {
                _methodName = "Constructor";
                _emamiContext = emamiContext;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        public ResultDto ValidateAppKey(KeyInputDto keyInputDto)
        {
            _methodName = "ValidateAppKey";
            try
            {
                keyInputDto.ClientType = "AppKey";
                return ClientKeyAuthentication(keyInputDto);
            }
            catch (Exception exception)
            {
                var resultDto = new ResultDto();
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto = new ErrorDto
                {
                    ErrorCode = Constants.Exception,
                    Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage)
                };
                _logger.Error(message);
                return resultDto;
            }


        }

        public ResultDto VerifyWebKey(KeyInputDto keyInputDto)
        {
            _methodName = "VerifyWebKey";
            try
            {
                keyInputDto.ClientType = "WebKey";
                return ClientKeyAuthentication(keyInputDto);
            }
            catch (Exception exception)
            {
                var resultDto = new ResultDto();
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto = new ErrorDto
                {
                    ErrorCode = Constants.Exception,
                    Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage)
                };
                _logger.Error(message);
                return resultDto;
            }


        }

        private ResultDto ClientKeyAuthentication(KeyInputDto keyInputDto)
        {
            _methodName = "ClientKeyAuthentication";
            var resultDto = new ResultDto();
            try
            {
                _logger.Info($"{ServiceName} Service-Method {_methodName}");
                if (!string.IsNullOrEmpty(keyInputDto.ClientKey))
                {
                    Guid outputGuid;
                    if (Guid.TryParse(keyInputDto.ClientKey, out outputGuid))
                    {
                        var configuration = _emamiContext.Configurations.FirstOrDefault(d => d.Key == keyInputDto.ClientType);
                        if (configuration != null)
                        {
                            if (string.Equals(configuration.Value, outputGuid.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                var newSystemToken = TokenManager.CreateJwtToken(new List<Claim>
                                {
                                    new Claim("System",
                                        EncryptDecryptHelper.Encrypt(SecurityConstants.KeyApiTokenKey
                                        ,SecurityConstants.EncryptionKey
                                        ,SecurityConstants.VectorKey))
                                });

                                resultDto.IsSuccess = true;
                                resultDto.SuccessDto = new SuccessDto
                                {
                                    Response = newSystemToken
                                };
                            }
                            else
                            {
                                resultDto.IsSuccess = false;
                                resultDto.ErrorDto = new ErrorDto
                                {
                                    ErrorCode = Constants.InValidClientKey,
                                    Message = Constants.GetMessage(Constants.InValidClientKey, Utility.MessageLanguage)
                                };
                                _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                            }
                        }
                        else
                        {
                            resultDto.IsSuccess = false;
                            resultDto.ErrorDto = new ErrorDto
                            {
                                ErrorCode = Constants.InValidClientKey,
                                Message = Constants.GetMessage(Constants.InValidClientKey, Utility.MessageLanguage)
                            };
                            _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                        }
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto = new ErrorDto
                        {
                            ErrorCode = Constants.InValidClientKey,
                            Message = Constants.GetMessage(Constants.InValidClientKey, Utility.MessageLanguage)
                        };
                        _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                    }

                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto = new ErrorDto
                    {
                        ErrorCode = Constants.ClientKeyCantBeEmpty,
                        Message = Constants.GetMessage(Constants.ClientKeyCantBeEmpty, Utility.MessageLanguage)
                    };
                    _logger.Debug($"{ServiceName} Controller-Method {_methodName} " + resultDto.ErrorDto.Message);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto = new ErrorDto
                {
                    ErrorCode = Constants.Exception,
                    Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage)
                };
                _logger.Error(message);
            }
            return resultDto;
        }
    }
}
