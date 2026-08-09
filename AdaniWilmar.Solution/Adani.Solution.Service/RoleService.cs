using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using GMCore.Logger;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Service
{
    public interface IRoleService
    {
        ResultDto GetSuadaBookingRestrictionRoleIds();
    }
    public class RoleService : IRoleService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Role Service");
        private const string ServiceName = "Role Service";
        private string _methodName;
        private readonly IResultService _resultService;
        public RoleService(IAdaniContext emamiContext, IResultService resultService) 
        { 
           _emamiContext = emamiContext;
           _resultService = resultService;
        }

        #region Booking restriction roles
        public ResultDto GetSuadaBookingRestrictionRoleIds()
        {
            _methodName = "GetSuadaBookingRestrictionRoleIds";
            var resultDto = new ResultDto();
            var rolelist = new List<DropDownDto>();
            try
            {
                rolelist = _emamiContext.Roles
                    .Where(_ => _.Id == (long)DTO.Enums.Role.Dealer || _.Id == (long)DTO.Enums.Role.StateTrader
                    || _.Id == (long)DTO.Enums.Role.ZonalTrader).Select(r => new DropDownDto()
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = rolelist;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

    }
}
