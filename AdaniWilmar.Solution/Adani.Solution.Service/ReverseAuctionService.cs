using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using Adani.Solution.Data.Entities;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GMCore.Helper;
using Adani.Solution.DTO.Enums;
using System.Web.Hosting;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Threading;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Globalization;

namespace Adani.Solution.Service
{
    public interface IReverseAuctionService
    {
        ResultDto AddOrUpdateBiddingWindowTiming(BiddingWindowTimingDto inputDto);
        ResultDto GetBiddingWindowTimingList(LoginUserIdDto inputDto);
        ResultDto GetBiddingWindowTimingById(long biddingWindowId);

        ResultDto AddOrUpdateTicker(TickerDto inputDto);
        ResultDto GetTickerList(LoginUserIdDto inputDto);
        ResultDto GetTickerById(long tickerId);
        ResultDto GetBiddingWindowTimingListddl(LoginUserIdDto inputDto);
        ResultDto GetBiddingWindowTimingListByDateddl(BiddingWindowInputDto inputDto);



    }

    public class ReverseAuctionService : IReverseAuctionService
    {

        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Reverse Auction Service");
        private const string ServiceName = "Reverse Auction Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public ReverseAuctionService(IAdaniContext salesContext, IResultService resultService, INotificationService notificationService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _notificationService = notificationService;
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Reverse Auction Service", exception);
            }
        }

        public ResultDto ReturnInvalidRequst()
        {
            var resultDto = new ResultDto();
            resultDto.IsSuccess = false;
            resultDto.ErrorDto.Message = Constants.InvalidRequest;
            return resultDto;
        }

        public ResultDto ReturnException(Exception exception)
        {
            var resultDto = new ResultDto();
            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
            _logger.Error(message);
            resultDto.IsSuccess = false;
            resultDto.ErrorDto.Message = Constants.Exception;
            return resultDto;
        }


        public ResultDto AddOrUpdateBiddingWindowTiming(BiddingWindowTimingDto inputDto)
        {
            _methodName = "AddOrUpdateBiddingWindowTiming";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    ReturnInvalidRequst();
                }

                if (inputDto.Id == 0 && inputDto.Date != null)
                {
                    var biddingData = _emamiContext.BiddingWindowTiming.AsNoTracking().FirstOrDefault(a => DbFunctions.TruncateTime(a.BiddingDate) == DbFunctions.TruncateTime(inputDto.Date) && a.IsLastWindowPerDay);
                    if (biddingData != null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.LastBiddingWindowClosed;
                        return resultDto;
                    }
                }

                if (inputDto.Id > 0)
                {
                    //var biddingWindowTimingExists = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(a => a.Id != inputDto.Id && DbFunctions.TruncateTime(a.BiddingDate) == DbFunctions.TruncateTime(inputDto.Date)
                    // && ((a.FromHours >= inputDto.From && a.FromHours <= inputDto.To) || (a.ToHours >= inputDto.From && a.ToHours <= inputDto.To))).Count();

                    var biddingWindowTimingExists = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(a => a.Id != inputDto.Id && DbFunctions.TruncateTime(a.BiddingDate) == DbFunctions.TruncateTime(inputDto.Date)
                    && inputDto.From <= a.ToHours && a.FromHours <= inputDto.To).Count();

                    if (biddingWindowTimingExists > 0)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.BiddingWindowExistsFromToHours;
                        return resultDto;
                    }
                    else
                    {
                        var biddingData = _emamiContext.BiddingWindowTiming.FirstOrDefault(a => a.Id == inputDto.Id);
                        biddingData.Id = inputDto.Id;
                        biddingData.BiddingDate = inputDto.Date;
                        biddingData.FromHours = inputDto.From;
                        biddingData.ToHours = inputDto.To;
                        biddingData.IsLastWindowPerDay = inputDto.IsLastWindowPerDay;
                        biddingData.Isactive = inputDto.IsActive;
                        biddingData.ModifiedBy = inputDto.LoginUserId;
                        biddingData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        //_emamiContext.BiddingWindowTiming.Add(biddingData);
                        _emamiContext.SaveChanges();
                    }
                }
                else
                {
                    //var biddingWindowTimingExists = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(a => DbFunctions.TruncateTime(a.BiddingDate) == DbFunctions.TruncateTime(inputDto.Date)
                    // && ((a.FromHours >= inputDto.From && a.FromHours <= inputDto.To) || (a.ToHours >= inputDto.From && a.ToHours <= inputDto.To))).Count();

                    var biddingWindowTimingExists = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(a => DbFunctions.TruncateTime(a.BiddingDate) == DbFunctions.TruncateTime(inputDto.Date)
                   && inputDto.From <= a.ToHours && a.FromHours <= inputDto.To).Count();

                    if (biddingWindowTimingExists > 0)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.BiddingWindowExistsFromToHours;
                        return resultDto;
                    }
                    else
                    {
                        var input = new BiddingWindowTiming
                        {
                            Id = inputDto.Id,
                            BiddingDate = inputDto.Date,
                            FromHours = inputDto.From,
                            ToHours = inputDto.To,
                            IsLastWindowPerDay = inputDto.IsLastWindowPerDay,
                            Isactive = inputDto.IsActive,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ModifiedBy = inputDto.LoginUserId,
                            ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.BiddingWindowTiming.Add(input);
                        _emamiContext.SaveChanges();
                    }
                }
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                return ReturnException(exception);
            }
        }

        public ResultDto GetBiddingWindowTimingList(LoginUserIdDto inputDto)
        {
            _methodName = "GetBiddingWindowTimingList";
            var resultDto = new ResultDto();
            var outputDto = new List<BiddingWindowTimingDto>();
            try
            {
                if (inputDto.IsToReturnInactiveData)
                {
                    var materialCost = _emamiContext.BiddingWindowTiming.AsNoTracking().ToList();
                    outputDto = materialCost.Select(c => new BiddingWindowTimingDto
                    {
                        Id = c.Id,
                        Date = c.BiddingDate,
                        From = c.FromHours,
                        FromTimeString = c.FromHours.ToString(),
                        ToTimeString = c.ToHours.ToString(),
                        To = c.ToHours

                    }).ToList();
                }
                else
                {
                    outputDto = _emamiContext.BiddingWindowTiming.AsNoTracking().Select(c => new BiddingWindowTimingDto
                    {
                        Id = c.Id,
                        Date = c.BiddingDate,
                        From = c.FromHours,
                        FromTimeString = c.FromHours.ToString(),
                        ToTimeString = c.ToHours.ToString(),
                        To = c.ToHours
                    }).ToList();
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                return ReturnException(exception);
            }
        }

        public ResultDto GetBiddingWindowTimingById(long biddingWindowId)
        {
            _methodName = "GetBiddingWindowTimingById";
            var resultDto = new ResultDto();
            try
            {
                var result = _emamiContext.BiddingWindowTiming.FirstOrDefault(f => f.Id == biddingWindowId);
                if (result != null)
                {
                    var biddingData = new BiddingWindowTimingDto()
                    {
                        Id = result.Id,
                        Date = result.BiddingDate,
                        From = result.FromHours,
                        To = result.ToHours,
                        IsActive = result.Isactive,
                        IsLastWindowPerDay = result.IsLastWindowPerDay
                    };
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = biddingData;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
            }
            catch (Exception exception)
            {
                return ReturnException(exception);
            }
            return resultDto;
        }


        public ResultDto AddOrUpdateTicker(TickerDto inputDto)
        {
            _methodName = "AddOrUpdateTicker";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    ReturnInvalidRequst();
                }

                if (inputDto.Id > 0)
                {
                    var tickerData = _emamiContext.Ticker.FirstOrDefault(a => a.Id == inputDto.Id);
                    tickerData.Id = inputDto.Id;
                    tickerData.Content = inputDto.Content;
                    tickerData.FromHours = inputDto.FromHours;
                    tickerData.ToHours = inputDto.ToHours;
                    tickerData.ColorCode = inputDto.Color;
                    //tickerData.CreatedDate = DateTime.UtcNow;
                    //tickerData.CreatedBy = inputDto.LoginUserId;
                    tickerData.TickerDate = inputDto.TickerDate;
                    tickerData.ModifiedBy = inputDto.LoginUserId;
                    tickerData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    tickerData.IsActive = inputDto.IsActive;
                    _emamiContext.SaveChanges();
                }
                else
                {
                    var input = new Ticker
                    {
                        Id = inputDto.Id,
                        TickerDate = inputDto.TickerDate,
                        Content = inputDto.Content,
                        FromHours = inputDto.FromHours,
                        ToHours = inputDto.ToHours,
                        ColorCode = inputDto.Color,
                            IsActive = inputDto.IsActive,
                        //ModifiedBy = inputDto.LoginUserId,
                        //ModifiedDate = DateTime.UtcNow,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        CreatedBy = inputDto.LoginUserId
                    };
                    _emamiContext.Ticker.Add(input);
                    _emamiContext.SaveChanges();
                }
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                return ReturnException(exception);
            }
        }

        public ResultDto GetTickerList(LoginUserIdDto inputDto)
        {
            _methodName = "GetTickerList";
            var resultDto = new ResultDto();
            var outputDto = new List<TickerDto>();
            try
            {
                if (inputDto.IsToReturnInactiveData)
                {
                    var tickers = _emamiContext.Ticker.AsNoTracking().ToList();
                    outputDto = tickers.AsEnumerable().Select(c => new TickerDto
                    {
                        EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                        Id = c.Id,
                        Content = c.Content,
                        FromHours = c.FromHours,
                        ToHours = c.ToHours,
                        TickerDate = c.TickerDate,
                        IsActive = c.IsActive,
                    }).ToList();
                }
                else
                {
                    outputDto = _emamiContext.Ticker.AsNoTracking().Select(c => new TickerDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        FromHours = c.FromHours,
                        ToHours = c.ToHours,
                        TickerDate = c.TickerDate,
                        IsActive = c.IsActive,
                    }).ToList();
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                return ReturnException(exception);
            }
        }

        public ResultDto GetTickerById(long tickerId)
        {
            _methodName = "GetTickerById";
            var resultDto = new ResultDto();
            try
            {
                var result = _emamiContext.Ticker.FirstOrDefault(f => f.Id == tickerId);
                if (result != null)
                {
                    var tickerData = new TickerDto()
                    {
                        Id = result.Id,
                        Content = result.Content,
                        FromHours = result.FromHours,
                        ToHours = result.ToHours,
                        TickerDate = result.TickerDate,
                        Color = result.ColorCode,
                        IsActive = result.IsActive,
                    };
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = tickerData;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
            }
            catch (Exception exception)
            {
                return ReturnException(exception);
            }
            return resultDto;
        }


        public ResultDto GetBiddingWindowTimingListddl(LoginUserIdDto inputDto)
        {
            _methodName = "GetBiddingWindowTimingListddl";
            var resultDto = new ResultDto();
            var outputDto = new List<DropDownDto>();
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var resultContext = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(currentDate)).ToList();
                outputDto = resultContext.Select(c => new DropDownDto
                {
                    Id = c.Id,
                    Name = c.FromHours.ToString() + " - " + c.ToHours.ToString()
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                return ReturnException(exception);
            }
        }

        public ResultDto GetBiddingWindowTimingListByDateddl(BiddingWindowInputDto inputDto)
        {
            _methodName = "GetBiddingWindowTimingListByDateddl";
            var resultDto = new ResultDto();
            var outputDto = new List<DropDownDto>();
            try
            {
                var resultContext = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(inputDto.BiddingDate)).ToList();
                outputDto = resultContext.Select(c => new DropDownDto
                {
                    Id = c.Id,
                    Name = c.FromHours.ToString() + " - " + c.ToHours.ToString()
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                return ReturnException(exception);
            }
        }

        

        

        public void InsertNotificationHistory(List<NotificationHistory> notificationHistories)
        {
            _emamiContext.BulkInsertProxy(notificationHistories);
            _emamiContext.SaveChanges();
        }
    }
}
