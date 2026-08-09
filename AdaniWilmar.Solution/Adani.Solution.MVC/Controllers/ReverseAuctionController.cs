using Adani.Solution.MVC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adani.Solution.DTO;
using Adani.Solution.MVC.ServiceClient;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using GMCore.Helper;
using Adani.Solution.MVC.Models;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.DTO.Enums;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using Adani.Solution.DTO.Common;
using Adani.Solution.MVC.Common;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class ReverseAuctionController : BaseController
    {
        private readonly ReverseAuctionClient _reverseAuctionClient;

        private readonly LookupClient _lookupClient;
        public ReverseAuctionController()
        {
            _reverseAuctionClient = new ReverseAuctionClient { ControllerDelegate = this };
        }

        #region BiddingWindow

        [AuthorizeClaims(Claims.ManageBiddingWindow)]
        public ActionResult BiddingWindow()
        {
            Session["BidWindowId"] = null;
            return View();
        }

        public async Task<ActionResult> GetBiddingWindowDetails([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _reverseAuctionClient.GetBiddingWindowTimingList(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [AuthorizeClaims(Claims.ManageBiddingWindow)]
        public async Task<ActionResult> BiddingWindowTiming()
        {
            var result = new BiddingWindowTimingDto();
            if (Session["BidWindowId"] != null && UtilityHelper.IntTryToParse(Session["BidWindowId"].ToString()) > 0)
            {
                result = await _reverseAuctionClient.BidddingWindowTiming(UtilityHelper.IntTryToParse(Session["BidWindowId"].ToString()));
            }
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> BiddingWindowTiming(BiddingWindowTimingDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _reverseAuctionClient.AddOrUpdateBidWindowTiming(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("BiddingWindow", "ReverseAuction");
            }
            return View(result);
        }

        public ActionResult BiddingWindowTimingEdit(int BidWindowId)
        {
            Session["BidWindowId"] = BidWindowId;
            return RedirectToAction("BiddingWindowTiming", "ReverseAuction");
        }

        public async Task<ActionResult> GetBiddingWindowDetailsddl()
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId };
            var result = await _reverseAuctionClient.GetBiddingWindowTimingListddl(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetBiddingWindowTimingListByDateddl(string biddingDate)
        {
            List<DropDownDto> result = new List<DropDownDto>();
            if (!string.IsNullOrEmpty(biddingDate))
            {
                BiddingWindowInputDto inputDto = new BiddingWindowInputDto { BiddingDate = Convert.ToDateTime(biddingDate) };
                var result1 = await _reverseAuctionClient.GetBiddingWindowTimingListByDateddl(inputDto);
                result = result1.ToList();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SaudhaDetails

        [AuthorizeClaims(Claims.ManageSauda)]
        public ActionResult SaudaList()
        {
            //var model = new SaudaUpdateDto();
            //if (Session["SaudaStatusMessage"] != null)
            //{
            //    model = (SaudaUpdateDto)Session["SaudaStatusMessage"];
            //    Session["SaudaStatusMessage"] = null;
            //}
            var verticalIdInModel = new SaudaUpdateDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId,
            };
            return View(verticalIdInModel);
        }

        public async Task<ActionResult> GetAllSaudhaList([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _reverseAuctionClient.GetAllSaudhaList(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult SaudhaDetailsEdit(string EncryptedId = "")
        {
            var saudaNo = "";
            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                saudaNo = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

            }
            Session["saudaNo"] = saudaNo;
            return RedirectToAction("SaudhaDetails", "ReverseAuction");
        }

        public async Task<ActionResult> SaudhaDetails()
        {
            var result = new SaudaListDto();
            if (Session["saudaNo"] != null && UtilityHelper.IntTryToParse(Session["saudaNo"].ToString()) > 0)
            {
                var saudaDto = new SaudaDetailInputDto { SaudaId = UtilityHelper.IntTryToParse(Session["saudaNo"].ToString()), UserId = this.UserId };
                result = await _reverseAuctionClient.GetSaudhaDetails(saudaDto);
            }
            result.EncryptedId = UtilityHelper.ConvertToMd5(result.SaudaId.ToString(), SecurityConstants.EncryptionKey);

            result.RoleId = RoleId;
            return View(result);
        }

        public async Task<ActionResult> SaudhaBrokerApproval()
        {
            var result = new SaudaListDto();
            if (Session["saudaNo"] != null && UtilityHelper.IntTryToParse(Session["saudaNo"].ToString()) > 0)
            {
                var saudaDto = new SaudaDetailInputDto { SaudaId = UtilityHelper.IntTryToParse(Session["saudaNo"].ToString()), UserId = this.UserId };
                result = await _reverseAuctionClient.GetSaudhaDetails(saudaDto);
            }
            return View(result);
        }

        public ActionResult SaudhaBrokerApprovalEdit(int saudaNo)
        {
            Session["saudaNo"] = 1;
            return RedirectToAction("SaudhaBrokerApproval", "ReverseAuction");
        }

        /// <summary>
        /// Method to Sauda details Approve/Reject action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> UpdateSaudhaStatus(SaudaUpdateDto saudaUpdateDto)
        {
            //Session["SaudaStatusMessage"] = null;
            saudaUpdateDto.ModifiedBy = UserId;
            saudaUpdateDto.LoginUserId = UserId;

            if (saudaUpdateDto.EncryptedIds.Count() > 0)
            {
                foreach (var id in saudaUpdateDto.EncryptedIds)
                {
                    var Id = id.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(Id, SecurityConstants.EncryptionKey);
                    saudaUpdateDto.SaudaOrderIds.Add(UtilityHelper.IntTryToParse(decryptedId));
                }
            }

            saudaUpdateDto = await _reverseAuctionClient.UpdateSaudaStatus(saudaUpdateDto);
            //if (saudaUpdateDto.PostStatus)
            //    Session["SaudaStatusMessage"] = saudaUpdateDto;
            return Json(saudaUpdateDto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> UpdateSaudhaStatusForLoose(SaudaUpdateDto saudaUpdateDto)
        {
            //Session["SaudaStatusMessage"] = null;
            saudaUpdateDto.ModifiedBy = UserId;
            saudaUpdateDto.LoginUserId = UserId;

            if (saudaUpdateDto.EncryptedIds.Count() > 0)
            {
                foreach (var id in saudaUpdateDto.EncryptedIds)
                {
                    var Id = id.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(Id, SecurityConstants.EncryptionKey);
                    saudaUpdateDto.SaudaOrderIds.Add(UtilityHelper.IntTryToParse(decryptedId));
                }
            }

            saudaUpdateDto = await _reverseAuctionClient.UpdateSaudhaStatusForLoose(saudaUpdateDto);
            //if (saudaUpdateDto.PostStatus)
            //    Session["SaudaStatusMessage"] = saudaUpdateDto;
            return Json(saudaUpdateDto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ReprocessSaudaConversion(SaudaConversionReprocessDto saudaConversionUpdateDto)
        {
            //Session["SaudaStatusMessage"] = null;
            saudaConversionUpdateDto.ModifiedBy = UserId;
            saudaConversionUpdateDto = await _reverseAuctionClient.ReprocessSaudaConversion(saudaConversionUpdateDto);
            //if (saudaUpdateDto.PostStatus)
            //    Session["SaudaStatusMessage"] = saudaUpdateDto;
            return Json(saudaConversionUpdateDto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> RejectSaudaConversion(SaudaConversionReprocessDto saudaConversionUpdateDto)
        {
            //Session["SaudaStatusMessage"] = null;
            saudaConversionUpdateDto.ModifiedBy = UserId;
            saudaConversionUpdateDto = await _reverseAuctionClient.RejectSaudaConversion(saudaConversionUpdateDto);
            //if (saudaUpdateDto.PostStatus)
            //    Session["SaudaStatusMessage"] = saudaUpdateDto;
            return Json(saudaConversionUpdateDto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ReprocessSaudaExtension(SaudaExtensionReprocessDto saudaExtensionUpdateDto)
        {
            //Session["SaudaStatusMessage"] = null;
            saudaExtensionUpdateDto.ModifiedBy = UserId;
            saudaExtensionUpdateDto.IsReprocess = true;
            saudaExtensionUpdateDto = await _reverseAuctionClient.ReprocessSaudaExtension(saudaExtensionUpdateDto);
            //if (saudaUpdateDto.PostStatus)
            //    Session["SaudaStatusMessage"] = saudaUpdateDto;
            return Json(saudaExtensionUpdateDto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ReprocessLiftingRequest(LiftingRequestReprocessDto inputDto)
        {
            //Session["SaudaStatusMessage"] = null;
            inputDto.ModifiedBy = UserId;
            inputDto.IsReprocess = true;

            if (inputDto.EncryptedIds.Count() > 0)
            {
                foreach (var id in inputDto.EncryptedIds)
                {
                    var Id = id.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(Id, SecurityConstants.EncryptionKey);
                    inputDto.LiftingIds.Add(UtilityHelper.IntTryToParse(decryptedId));
                }
            }

            inputDto = await _reverseAuctionClient.ReprocessLiftingRequest(inputDto);
            //if (saudaUpdateDto.PostStatus)
            //    Session["SaudaStatusMessage"] = saudaUpdateDto;
            return Json(inputDto, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lifting List

        [AuthorizeClaims(Claims.ManageLiftingList)]
        public ActionResult LiftingList()
        {
            var inputDto = new LiftingRequestOutputDto()
            {
                VerticalId = VerticalId,
                RoleId = RoleId,
            };
            return View(inputDto);
        }

        public async Task<ActionResult> LiftingDetails()
        {
            var result = new LiftingRequestWebDto();
            if (Session["LiftingId"] != null && UtilityHelper.IntTryToParse(Session["LiftingId"].ToString()) > 0)
            {
                IdInputDto idInputDto = new IdInputDto { Id = Convert.ToInt32(Session["LiftingId"].ToString()) };
                result = await _reverseAuctionClient.GetLiftingDetails(idInputDto);
            }
            return View(result);
        }

        [NoCache]
        public async Task<JsonResult> LiftingDetailsById([DataSourceRequest] DataSourceRequest request, int LiftingRequestId)
        {
            List<LiftingRequestDetailsOutputDto> LiftingRequestDetailList = new List<LiftingRequestDetailsOutputDto>();
            if (LiftingRequestId > 0)
            {
                var result = new LiftingRequestWebDto();
                IdInputDto idInputDto = new IdInputDto { Id = LiftingRequestId };
                result = await _reverseAuctionClient.GetLiftingDetails(idInputDto);

                if (result.LiftingRequestDetailList != null)
                {
                    LiftingRequestDetailList = result.LiftingRequestDetailList;
                    var resultList = LiftingRequestDetailList.ToDataSourceResult(request);
                    resultList.Total = LiftingRequestDetailList.Count;
                    return Json(resultList);
                }
            }
            return Json(LiftingRequestDetailList);
        }

        public ActionResult LDEdit(string EncryptedId = "")
        {
            EncryptedId = EncryptedId.Replace(' ', '+');
            var decryptedId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);
            Session["LiftingId"] = decryptedId;
            return RedirectToAction("LiftingDetails", "ReverseAuction");
        }

        /// <summary>
        /// Method to PJP post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> LiftingRequestStatusChange(LiftingRequestStatusChangeDto liftingRequestStatusChange)
        {
            liftingRequestStatusChange.LoginUserId = UserId;
            liftingRequestStatusChange = Helper.SanitizeModel(liftingRequestStatusChange);
            liftingRequestStatusChange = await _reverseAuctionClient.LiftingRequestStatusChange(liftingRequestStatusChange);
            return Json(liftingRequestStatusChange, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetAllLiftingRequestList([DataSourceRequest] DataSourceRequest request, DealersLiftingRequestInputDto inputDto)
        {
            List<LiftingRequestOutputDto> result = new List<LiftingRequestOutputDto>();
            //if (inputDto.StateIds != null && inputDto.StateIds.Any())
            //{
            inputDto.DataSourceRequest = request;
            //}
            inputDto.DealerId = UserId;
            inputDto.LoginUserId = UserId;
            inputDto.RoleId = RoleId;

            var results = await _reverseAuctionClient.GetLiftingRequestList(inputDto);

            return Json(results);
        }

        #endregion

        #region Trade Ticket

        [AuthorizeClaims(Claims.ManageTradeTicket, Claims.ViewTradeTicket)]
        public ActionResult TradeTicketStatus()
        {
            var result = new RoleIdDto()
            {
                RoleId = RoleId,
                VerticalId = VerticalId,
            };
            return View(result);
        }

        public async Task<ActionResult> TradeTicketStatusDataAsync([DataSourceRequest] DataSourceRequest request, TradeTicketStatusSearchDto tradeTickerStatusSearchDto)
        {
            tradeTickerStatusSearchDto.LoginUserId = UserId;
            //TradeTickerStatusSearchDto searchDto = new TradeTickerStatusSearchDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, SearchDate=searchDate };
            var result = await _reverseAuctionClient.TradeTicketStatusList(tradeTickerStatusSearchDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> TradeTicketSaudaDetail([DataSourceRequest] DataSourceRequest request, long TradeTicketId)
        {
            IdInputDto idInputDto = new IdInputDto { LoginUserId = UserId, Id = TradeTicketId };
            var result = await _reverseAuctionClient.TradeTicketSaudaDetail(idInputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult TradeTicketDetails()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ManageTradeTicket, Claims.ViewTradeTicket)]
        public ActionResult TradeTicketList()
        {
            return View();
        }

        public async Task<ActionResult> TradeTicketListDataAsync([DataSourceRequest] DataSourceRequest request, TradeTicketParamDto tradeTicketParamDto)
        {
            tradeTicketParamDto.LoginUserId = UserId;
            // var loginUserIdDto = new TradeTicketParamDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, SearchDate = searchDate };
            if (tradeTicketParamDto.Vertical == "SF")
            {
                tradeTicketParamDto.VerticalId = (int)DTO.Enums.Division.SpecialityFat;
            }
            else if (tradeTicketParamDto.Vertical == "HBC")
            {
                tradeTicketParamDto.VerticalId = (int)DTO.Enums.Division.Hbc;
            }
            else
            {
                tradeTicketParamDto.VerticalId = (int)DTO.Enums.LooseVertical.Loose;
            }
            var result = await _reverseAuctionClient.GetAllTradeTicket(tradeTicketParamDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult EditTradeTicketandRedirect(int tradeTicketId = 0)
        {
            Session["TradeTicketId"] = tradeTicketId;
            return RedirectToAction("AddOrUpdateTradeTciket", "ReverseAuction");

        }

        public async Task<ActionResult> DeleteTradeTicket(int tradeTicketId)
        {
            var result = await _reverseAuctionClient.DeleteTradeTicket(tradeTicketId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeClaims(Claims.ManageTradeTicket, Claims.ViewTradeTicket)]
        public async Task<ActionResult> AddOrUpdateTradeTciket()
        {
            //return View(new TradeTicketInputDto());
            var result = new TradeTicketViewDto();
            if (Session["TradeTicketId"] != null && UtilityHelper.IntTryToParse(Session["TradeTicketId"].ToString()) > 0)
            {
                var dto = new TradeTicketInputDto() { TradeTicketId = UtilityHelper.LongTryToParse(Session["TradeTicketId"].ToString()), LoginUserId = UserId };
                result = await _reverseAuctionClient.GetTradeTicket(dto);
            }
            else
            {
                result = new TradeTicketViewDto();
            }
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdateTradeTciket(TradeTicketInputDto dto)
        {
            dto.LoginUserId = UserId;
            var res = await _reverseAuctionClient.AddOrUpdateTradeTciket(dto);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeClaims(Claims.ManageTradeTicket, Claims.ViewTradeTicket)]
        public ActionResult TradeTicketListRA()
        {
            return View();
        }
        public ActionResult EditTradeTicketandRedirectRA(int tradeTicketId = 0)
        {
            Session["TradeTicketId"] = tradeTicketId;
            return RedirectToAction("AddOrUpdateTradeTciketRA", "ReverseAuction");

        }

        [AuthorizeClaims(Claims.ManageTradeTicket, Claims.ViewTradeTicket)]
        public async Task<ActionResult> AddOrUpdateTradeTciketRA()
        {
            //return View(new TradeTicketInputDto());
            var result = new TradeTicketViewDto();
            if (Session["TradeTicketId"] != null && UtilityHelper.IntTryToParse(Session["TradeTicketId"].ToString()) > 0)
            {
                var dto = new TradeTicketInputDto() { TradeTicketId = UtilityHelper.LongTryToParse(Session["TradeTicketId"].ToString()), LoginUserId = UserId };
                result = await _reverseAuctionClient.GetTradeTicket(dto);
            }
            else
            {
                result = new TradeTicketViewDto();
            }
            return View(result);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdateTradeTciketRA(TradeTicketInputDto dto)
        {
            dto.LoginUserId = UserId;
            var res = await _reverseAuctionClient.AddOrUpdateTradeTciket(dto);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeClaims(Claims.ManageTradeTicket, Claims.ViewTradeTicket)]
        public ActionResult TradeTicketListLoose()
        {
            return View();
        }
        public ActionResult EditTradeTicketandRedirectLoose(int tradeTicketId = 0)
        {
            Session["TradeTicketId"] = tradeTicketId;
            return RedirectToAction("AddOrUpdateTradeTciketLoose", "ReverseAuction");

        }

        [AuthorizeClaims(Claims.ManageTradeTicket, Claims.ViewTradeTicket)]
        public async Task<ActionResult> AddOrUpdateTradeTciketLoose()
        {
            //return View(new TradeTicketInputDto());
            var result = new TradeTicketViewDto();
            if (Session["TradeTicketId"] != null && UtilityHelper.IntTryToParse(Session["TradeTicketId"].ToString()) > 0)
            {
                var dto = new TradeTicketInputDto() { TradeTicketId = UtilityHelper.LongTryToParse(Session["TradeTicketId"].ToString()), LoginUserId = UserId };
                result = await _reverseAuctionClient.GetTradeTicket(dto);
            }
            else
            {
                result = new TradeTicketViewDto();
            }
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdateTradeTciketLoose(TradeTicketInputDto dto)
        {
            dto.LoginUserId = UserId;
            var res = await _reverseAuctionClient.AddOrUpdateTradeTciket(dto);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ComparativeAnalysis()
        {
            return View();
        }

        [AuthorizeClaims(Claims.ManageTradeTicket, Claims.ViewTradeTicket)]
        public async Task<ActionResult> SaudaMapping()
        {
            var tradeTicketSaudaMappingDto = new TradeTicketSaudaMappingDto();
            if (Session["TradeTicketId"] != null && UtilityHelper.IntTryToParse(Session["TradeTicketId"].ToString()) > 0)
            {
                //model.TradeTicketId = UtilityHelper.IntTryToParse(Session["TradeTicketId"].ToString());
                IdInputDto inputDto = new IdInputDto { Id = UtilityHelper.IntTryToParse(Session["TradeTicketId"].ToString()) };
                tradeTicketSaudaMappingDto = await _reverseAuctionClient.GetSaudaOrdersTradeTicketMappingDetails(inputDto);
            }
            return View(tradeTicketSaudaMappingDto);
        }

        [AuthorizeClaims(Claims.ManageTradeTicket, Claims.ViewTradeTicket)]
        public ActionResult EditSaudaMapping(long tradeTicketId = 0)
        {
            Session["TradeTicketId"] = tradeTicketId;
            return RedirectToAction("SaudaMapping", "ReverseAuction");
        }

        public async Task<ActionResult> TradeTicketDropdownList(bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var res = await _reverseAuctionClient.TradeTicketDropDownList(loginUserIdDto);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaudaOrderList([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _reverseAuctionClient.SaudaOrderList(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> TradeTicketSaudaOrderList([DataSourceRequest] DataSourceRequest request, TradeTicketSaudaSearchDto tradeTicketSaudaSearchDto)
        {
            //var idinput = new IdInputDto();
            //idinput.Id = TradeTicketId;
            var result = await _reverseAuctionClient.GetTradeTicketSaudaOrdersMappingList(tradeTicketSaudaSearchDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        [HttpPost]
        public async Task<ActionResult> MapTradeTicketToSaudaOrders(TradeTicketMaptoSaudaOrderDto dto)
        {
            dto.LoginUserId = UserId;
            var result = await _reverseAuctionClient.MapTradeTicketToSaudaOrders(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSaudaListForAdmin([DataSourceRequest] DataSourceRequest request, DateTime fromDate, DateTime todate, int statusId, int dataFilter, long divisionId, long salesOrganizationId, long DistributionChannelId, long OilTypeId, long SkuId, long ZoneId, long StateId, long DistrictId, long CityId)
        {
            //request.Filters = Utility.ToFilterDescriptor(request.Filters);
            var saudaFilterDto = new SaudaListFilterDto() { LoginUserId = UserId, RoleId = RoleId, FromDate = fromDate, ToDate = todate, StatusId = statusId, DataFilter = dataFilter, SalesOrganizationId = salesOrganizationId, DistributionChannelId = DistributionChannelId, DivisionId = divisionId, DataSourceRequest = request, OilTypeId = OilTypeId, SkuId = SkuId, ZoneId = ZoneId, StateId = StateId, DistrictId = DistrictId, CityId = CityId };
            var result = await _reverseAuctionClient.GetSaudaListForAdminAsync(saudaFilterDto);
            if (result != null)
            {
                if (result != null && result.Data != null)
                {
                    var saudaList = result.Data as List<SaudaListDto>;
                    if (saudaList != null && saudaList.Any())
                    {
                        saudaList.ForEach(f =>
                        {
                            f.BiddingDate = ConvertUTCToIndiaTime(f.BiddingDate);
                            f.DataFilter = dataFilter;
                        });
                        result.Data = saudaList;
                    }
                }
            }
            return Json(result);
            //var resultList = result.ToDataSourceResult(request);            
        }

        public async Task<ActionResult> GetSaudaListExport(DateTime fromDate, DateTime todate, int statusId, int dataFilter, long divisionId, long salesOrganizationId, long DistributionChannelId, long OilTypeId, long SkuId)
        {
            //request.Filters = Utility.ToFilterDescriptor(request.Filters);
            var saudaFilterDto = new SaudaListFilterDto() { LoginUserId = UserId,RoleId=RoleId, FromDate = fromDate, ToDate = todate, StatusId = statusId, DataFilter = dataFilter, SalesOrganizationId = salesOrganizationId, DistributionChannelId = DistributionChannelId, DivisionId = divisionId, OilTypeId = OilTypeId, SkuId = SkuId };
            var result = await _reverseAuctionClient.GetSaudaListExport(saudaFilterDto);
            string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
            //var saudaResult = _reportClient.GetRaSaudaOrderReport(fromDate, toDate, stateIds, verticalId, statusIds);

            string fileName = "SaudaList_" + string.Format(Settings.ReportDateFormat, DateTime.Now.Date).ToUpper() + ".xlsx";
            bool isHeaderBind = false;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                var rowIndex = 7;
                var colIndex = 1;
                var childColIndex = 0;

                #region Header


                worksheet.Cells["A1:M1"].Merge = true;
                worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                worksheet.Cells["A2"].Value = "Report Name";
                worksheet.Cells["A3"].Value = "From date";
                worksheet.Cells["A4"].Value = "To Date";

                worksheet.Cells["B2"].Value = "Sauda List Details";
                worksheet.Cells["B3"].Value = string.Format(Settings.ReportDateFormat, fromDate);
                worksheet.Cells["B4"].Value = string.Format(Settings.ReportDateFormat, todate);

                for (int i = 2; i <= 4; i++)
                {
                    worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Cells["A" + i].Style.Font.Bold = true;
                    worksheet.Cells["A" + i].Style.Font.Size = 12;

                    worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                    worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                }

                #endregion

                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaNumber"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BookedNo"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BiddingDate"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DealerName"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Zone"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_State"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_District"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_City"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CreatedBy"));
                GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "SaudaType");
                //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SAPCreationDate"));
                //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_RatePerMT"));

                foreach (var saudhaList in result)
                {
                    isHeaderBind = false;
                    rowIndex++;
                    colIndex = 1;
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.SaudaNumber);
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.SaudaId.ToString());
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.BiddingDate.ToString());
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.DealerName.ToString());
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.Zones);
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.States);
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.Districts);
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.Cities);
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.CreatedBy);
                    GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.SaudaType);
                    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.saudhaListOilTypes);
                    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.SAPCreationDate != null ? saudhaList.SAPCreationDate.ToString(Settings.DateFormat) : string.Empty);
                    //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], saudhaList.RatePerMT.ToString());

                    if (saudhaList.InnerList != null && saudhaList.InnerList.Any())
                    {
                        foreach (var saudaorders in saudhaList.InnerList)
                        {
                            if (!isHeaderBind)
                            {
                                rowIndex++;
                                childColIndex = 2;

                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Plant"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_OilType"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SkuName"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], "Valid From");
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], "Valid To");
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_QuantityPerMT"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_QuantityPerCase"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], "Price Before Discount/Premium Applied");
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_FinalPrice"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SaudaBidPricePerCase"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_DiscountType"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_DiscountAmount"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_IncoTerms"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_State"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_BDOName"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_BDOCode"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_DealerCode"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Status"));
                                GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Remarks"));
                                isHeaderBind = true;
                            }
                            rowIndex++;
                            childColIndex = 2;
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.PlantName);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.OiltypeName);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.SkuName);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.SkuCode.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.ValidFromDate.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.ValidToDate.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.BidQuantity.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.BidQuantityCase.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.BidPrice.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.QuotedPrice.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.BidPricePerCase.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.DiscountType.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.DiscountAmount.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.Incoterms1.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.StateName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.BDOName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.BDOCode.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.DealerCode.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.Status != null ? saudaorders.Status : String.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], saudaorders.Remarks != null ? saudaorders.Remarks : String.Empty);
                        }
                    }
                }
                worksheet.Cells.AutoFitColumns();
                return SaveExcelFileToPath(package, fileName);
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
            //var resultList = result.ToDataSourceResult(request);            
        }
        public string SaveExcelFileToPath(ExcelPackage excelPackage)
        {
            //_methodName = "SaveExcelFileToPath";
            //_logger.Info($"{ServiceName} Controller-Method {_methodName}");

            string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
            string fileGuid = Guid.NewGuid().ToString();
            string guidFileName = fileGuid + ".xlsx";
            string savePath = Path.Combine(serverFoloderPath, guidFileName);

            if (System.IO.File.Exists(savePath))
            {
                System.IO.File.Delete(savePath);
                using (Stream stream = System.IO.File.Create(savePath))
                {
                    excelPackage.SaveAs(stream);
                }
            }
            else
            {
                using (Stream stream = System.IO.File.Create(savePath))
                {
                    excelPackage.SaveAs(stream);
                }
            }
            return guidFileName;
        }

        public async Task<ActionResult> GetDealersListByStateIdAsync([DataSourceRequest] DataSourceRequest request, string StateIds)
        {
            IList<DropDownDto> dealerList = new List<DropDownDto>();
            List<string> id = StateIds.Split(',').ToList();
            List<int> idIntList = id.ConvertAll(int.Parse);

            dealerList = await _reverseAuctionClient.GetDealersListByStateIdAsync(idIntList);
            return Json(dealerList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Ticker 

        public async Task<ActionResult> AddorUpdateTicker()
        {
            var result = new TickerDto();
            if (Session["TickerId"] != null && UtilityHelper.IntTryToParse(Session["TickerId"].ToString()) > 0)
            {
                var dto = new TradeTicketInputDto() { TradeTicketId = UtilityHelper.LongTryToParse(Session["TickerId"].ToString()), LoginUserId = UserId };
                result = await _reverseAuctionClient.GetTicker(UtilityHelper.LongTryToParse(Session["TickerId"].ToString()));

                result.EncryptedId = UtilityHelper.ConvertToMd5(result.Id.ToString(), SecurityConstants.EncryptionKey);
            }
            else
            {
                result = new TickerDto();
            }
            return View(result);
        }

        [AuthorizeClaims(Claims.ManageOrganization)]
        [HttpPost]
        public async Task<ActionResult> AddorUpdateTicker(TickerDto inputDto)
        {
            inputDto.LoginUserId = UserId;

            if (!String.IsNullOrEmpty(inputDto.EncryptedId))
            {
                inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                inputDto.Id = UtilityHelper.IntTryToParse(decryptedId);
            }

            var result = await _reverseAuctionClient.AddOrUpdateTicker(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("TickerList", "ReverseAuction");
            }
            return View(result);
        }

        public async Task<ActionResult> TickerListDataAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _reverseAuctionClient.GetTickerList(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public ActionResult TEdit(string EncryptedId="")
        {
            var tickerId = "";

            if (!String.IsNullOrEmpty(EncryptedId))
            {
                EncryptedId = EncryptedId.Replace(' ', '+');
                tickerId = UtilityHelper.ConvertMd5ToString(EncryptedId, SecurityConstants.EncryptionKey);

            }
            Session["TickerId"] = tickerId;
            return RedirectToAction("AddorUpdateTicker", "ReverseAuction");
        }

        [AuthorizeClaims(Claims.ManageOrganization)]
        public ActionResult TickerList()
        {
            return View();
        }

        #endregion

        /// <summary>
        /// Method to PJP post action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> LiftingRequestStatusChanges(LiftingRequestStatusChangeDto liftingRequestStatusChange)
        {
            liftingRequestStatusChange.LoginUserId = UserId;
            liftingRequestStatusChange = Helper.SanitizeModel(liftingRequestStatusChange);

            if (liftingRequestStatusChange.EncryptedIds.Count() > 0)
            {
                foreach (var id in liftingRequestStatusChange.EncryptedIds)
                {
                    var Id = id.Replace(' ', '+');
                    var decryptedId = UtilityHelper.ConvertMd5ToString(Id, SecurityConstants.EncryptionKey);
                    liftingRequestStatusChange.LiftingIds.Add(UtilityHelper.IntTryToParse(decryptedId));
                }
            }

            liftingRequestStatusChange = await _reverseAuctionClient.LiftingRequestStatusChanges(liftingRequestStatusChange);
            return Json(liftingRequestStatusChange, JsonRequestBehavior.AllowGet);
        }

        #region Lifting request Custom Export

        public async Task<ActionResult> GetLiftingRequestListForExport(DealersLiftingRequestInputDto inputDto)
        {
            var stream = new MemoryStream();
            var result = new ResultModel { IsSuccess = false, Message = "Error Occured while exporting Excel. Please retry." };
            try
            {
                inputDto.DealerId = UserId;
                var liftingDetails = await _reverseAuctionClient.GetLiftingRequestListForExport(inputDto);

                var fileName = $"SalesOrder_{DateTime.Now}.xlsx";

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage())
                {
                    // add a new worksheet to the empty workbook
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    var rowIndex = 1;
                    var colIndex = 1;
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IndentRequestNumber"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IndentRequestDate"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DealerName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IndentQuantityInMT"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IndentQuantityInCase"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Status"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuName"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DeliveryOrderNumber"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TotalQuantityInMT"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TotalQuantityInCase"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Remarks"));
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CustomerRemarks"));

                    foreach (var liftingResult in liftingDetails)
                    {
                        rowIndex++;
                        colIndex = 1;
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.LiftingRequestNumber != null ? liftingResult.LiftingRequestNumber.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.LiftingRequestdate != null ? liftingResult.LiftingRequestdate.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.DealerName != null ? liftingResult.DealerName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.LiftingQuantity.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.LiftingQuantityCase.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.StatusName != null ? liftingResult.StatusName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.SkuName != null ? liftingResult.SkuName.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.SkuCode != null ? liftingResult.SkuCode.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.OilType != null ? liftingResult.OilType.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.DeliveryOrderNumber != null ? liftingResult.DeliveryOrderNumber.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.TotalQuantity.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.TotalQuantityInCase.ToString());
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.ApproverRemarks != null ? liftingResult.ApproverRemarks.ToString() : string.Empty);
                        GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.CustomerRemarks != null ? liftingResult.CustomerRemarks.ToString() : string.Empty);
                    }

                    foreach (var workSheet in package.Workbook.Worksheets)
                    {
                        for (var i = 1; i <= workSheet.Dimension.End.Column; i++)
                        {
                            try
                            {
                                workSheet.Column(i).AutoFit();
                                workSheet.Column(i).BestFit = true;
                            }
                            catch
                            {
                                // ignored
                            }
                        }

                        try
                        {
                            var cells = workSheet.Cells[workSheet.Dimension.Address];
                            cells.AutoFitColumns();
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", fileName));
                    this.Response.BinaryWrite(package.GetAsByteArray());
                }
                result.IsSuccess = true;
                result.Message = fileName;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Excel Error" + ex;
            }
            Response.Flush();
            Response.End();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private ExcelRange GetExcelTitle(ExcelRange cell, string title, int level = 0)
        {
            cell.Value = title ?? string.Empty;

            cell.Style.Border.Top.Style =
                cell.Style.Border.Left.Style =
                    cell.Style.Border.Right.Style = cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Color.SetColor(Color.White);
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
            if (level > 0)
            {
                cell.Style.Fill.BackgroundColor.SetColor(Color.Green);
            }
            return cell;
        }

        private ExcelRange GetExcelContent(ExcelRange cell, string text, int align = 0)
        {
            cell.Value = text ?? string.Empty;

            cell.Style.Border.Top.Style =
                cell.Style.Border.Left.Style =
                    cell.Style.Border.Right.Style = cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            if (align == 1)//align right
            {
                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            }

            return cell;
        }

        #endregion

        #region Sauda

        [NoCache]
        public async Task<ActionResult> GetSaudaOrderLiftingRequestDetails([DataSourceRequest] DataSourceRequest request, long liftingRequestDetailId)
        {
            IdInputDto idInputDto = new IdInputDto() { Id = liftingRequestDetailId };
            var result = await _reverseAuctionClient.GetSaudaOrderLiftingRequestDetails(idInputDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> GetSaudaOrderLiftingRequestExcelExport(DealersLiftingRequestInputDto inputDto)
        {
            var finalResult = new JsonResult();
            try
            {
                string fileName = "SalesOrderDetails_" + string.Format(Settings.ReportDateFormat, DateTime.Now.Date).ToUpper() + ".xlsx";
                bool isHeaderBind = false;
                inputDto.DealerId = UserId;
                inputDto.LoginUserId = UserId;
                inputDto.RoleId = RoleId;
                var liftingRequestDetails = await _reverseAuctionClient.GetSaudaOrderLiftingRequestExcelExport(inputDto);

                if (liftingRequestDetails != null && liftingRequestDetails.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        var rowIndex = 7;
                        var colIndex = 1;
                        var childColIndex = 0;

                        #region Header

                        worksheet.Cells["A1:M1"].Merge = true;
                        worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["A3"].Value = "From date";
                        worksheet.Cells["A4"].Value = "To Date";
                        worksheet.Cells["A5"].Value = "Status";

                        string statusName = inputDto.StatusId == -1
                            ? Utility.GetEnumFromString<Status>((int)Status.Pending, (int)Status.Approved)
                            : Utility.GetEnumFromString<Status>(inputDto.StatusId); ;

                        worksheet.Cells["B2"].Value = "Sales Order Details";
                        worksheet.Cells["B3"].Value = string.Format(Settings.ReportDateFormat, inputDto.FromDate);
                        worksheet.Cells["B4"].Value = string.Format(Settings.ReportDateFormat, inputDto.ToDate);
                        worksheet.Cells["B5"].Value = statusName;

                        for (int i = 2; i <= 5; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        #endregion

                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IndentRequestNumber"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IndentRequestDate"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DealerName"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Plant Code");
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Plant Name");
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CreatedBy"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sales Order Quantity" + " (MT)");
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Sales Order Quantity");
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Status"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ApproverRemarks"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CustomerRemarks"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_EnquiryNumber"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_DeliveryOrderNumber"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_EnquiryRemarks"));

                        foreach (var liftingResult in liftingRequestDetails)
                        {
                            isHeaderBind = false;
                            rowIndex++;
                            colIndex = 1;
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.LiftingRequestId);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.LiftingRequestNumber);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.LiftingRequestdate != null ? liftingResult.LiftingRequestdate.ToString() : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.Dealer);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.PlantCode);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.PlantName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.CreatedUser);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.RequestedQuantity.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.RequestedQuantityInCase.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.Status);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.Remarks);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.CustomerRemarks);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.EnquiryNumber);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.DeliveryOrderNumber);
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], liftingResult.EnquiryRemarks);

                            if (liftingResult.LiftingRequestDetails != null && liftingResult.LiftingRequestDetails.Any())
                            {
                                foreach (var lifting in liftingResult.LiftingRequestDetails)
                                {
                                    if (!isHeaderBind)
                                    {
                                        rowIndex++;
                                        childColIndex = 2;
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SkuName"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SkuCode"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_OilType"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], "Sales Order Quantity (MT)");
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], "Sales Order Quantity");
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], "Contract Number");
                                        isHeaderBind = true;
                                    }
                                    rowIndex++;
                                    childColIndex = 2;
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], lifting.SkuName);
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], lifting.SkuCode);
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], lifting.OilType);
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], lifting.LiftingQuantity.ToString());
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], lifting.LiftingQuantityCase.ToString());
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], lifting.SaudaNumber == null ? string.Empty : lifting.SaudaNumber);
                                }
                            }
                        }
                        worksheet.Cells.AutoFitColumns();
                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SaudaDetailsUpdate(SaudaListDto inputDto)
        {
            var data = JsonConvert.DeserializeObject<List<SaudaListDto>>(inputDto.SaudaListString, UtilityHelper.GetJsonSettings());
            var result = new SaudaDetailOutputDto();
            foreach (var sauda in data)
            {
                var saudaInput = new SaudaDetailOutputDto()
                {
                    DiscountAmount = sauda.DiscountAmount,
                    DiscountTypeId = sauda.DiscountTypeId,
                    DealerId = sauda.DealerId,
                    SkuId = sauda.SkuId,
                    BidQuantity = sauda.BidQuantity,
                    SalesOrganizationId = sauda.SalesOrganizationId,
                    DistributionChannelId = sauda.DistributionChannelId,
                    DivisionId = sauda.DivisionId,
                    BidPrice = sauda.BidPrice,
                    QuotedPrice = sauda.QuotedPrice,
                    BidPricePerCase = sauda.BidPricePerCase,
                    SaudaOrderId = sauda.SaudaOrderId,
                    BidQuantityCase = sauda.BidQuantityCase,
                    BasePricePerCase = sauda.BasePricePerCase,
                    LoginUserId = UserId
                };
                result = await _reverseAuctionClient.UpdateSaudaDetails(saudaInput);
            }

            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                var verticalIdInModel = new SaudaUpdateDto()
                {
                    VerticalId = VerticalId
                };
                return View("SaudaList", verticalIdInModel);
            }
            else
            {
                TempData["ErrorMessage"] = result.PostMessage;
            }
            //var inputData = JsonHelper.ConvertJSonToObjectList<SaudaListDto>(inputDto);
            //inputDto.LoginUserId = UserId;
            //var result = await _reverseAuctionClient.UpdateSaudaDetails(inputDto);
            //if (result.PostStatus)
            //{
            //    TempData["SuccessMessage"] = result.PostMessage;
            //    var verticalIdInModel = new SaudaUpdateDto()
            //    {
            //        VerticalId = VerticalId
            //    };
            //    return View("SaudaList", verticalIdInModel);
            //}
            return View("SaudaList");

        }

        [HttpPost]
        public async Task<ActionResult> SaudaDetails(SaudaDetailOutputDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _reverseAuctionClient.UpdateSaudaDetails(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                var verticalIdInModel = new SaudaUpdateDto()
                {
                    VerticalId = VerticalId
                };
                return View("SaudaList", verticalIdInModel);
            }
            else
            {
                var saudaDetail = new SaudaListDto();
                if (Session["saudaNo"] != null && UtilityHelper.IntTryToParse(Session["saudaNo"].ToString()) > 0)
                {
                    var saudaDto = new SaudaDetailInputDto { SaudaId = UtilityHelper.IntTryToParse(Session["saudaNo"].ToString()), UserId = this.UserId };
                    saudaDetail = await _reverseAuctionClient.GetSaudhaDetails(saudaDto);
                }
                TempData["ErrorMessage"] = result.PostMessage;
                return View("SaudhaDetails", saudaDetail);
            }
        }

        #endregion

        #region Export Trade Ticket Status - Custom Export

        public async Task<ActionResult> ExcelExportTradeTicketStatus(TradeTicketSearchDto inputDto)
        {
            var finalResult = new JsonResult();
            try
            {
                string fileName = "TradeTicketStatus_" + string.Format(Settings.ReportDateFormat, DateTime.Now.Date).ToUpper() + ".xlsx";
                bool isHeaderBind = false;
                var tradeTicketDetails = await _reverseAuctionClient.ExcelExportTradeTicketStatus(inputDto);

                if (tradeTicketDetails != null && tradeTicketDetails.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        var rowIndex = 7;
                        var colIndex = 1;
                        var childColIndex = 0;

                        #region Header

                        worksheet.Cells["A1:M1"].Merge = true;
                        worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["A3"].Value = "From date";
                        worksheet.Cells["A4"].Value = "To Date";

                        worksheet.Cells["B2"].Value = "Trade Ticket Details";
                        worksheet.Cells["B3"].Value = string.Format(Settings.ReportDateFormat, inputDto.FromDate);
                        worksheet.Cells["B4"].Value = string.Format(Settings.ReportDateFormat, inputDto.ToDate);

                        for (int i = 2; i <= 4; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        #endregion

                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TradeTicketNumber"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TotalQuantityInMT"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaBookedQuantityInMT"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OpenQuantityInMT"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Plant"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SAPCreationDate"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_RatePerMT"));

                        foreach (var tradeTicket in tradeTicketDetails)
                        {
                            isHeaderBind = false;
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.TradeTicketNumber);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.TotalQuantity.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.SaudaQuantity.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.OpenQty.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.PlantName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.TradeTicketOilTypes);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.SAPCreationDate != null ? tradeTicket.SAPCreationDate.ToString(Settings.DateFormat) : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.RatePerMT.ToString());

                            if (tradeTicket.TradeTicketDetailList != null && tradeTicket.TradeTicketDetailList.Any())
                            {
                                foreach (var tradeTicketSaudaOrderDetail in tradeTicket.TradeTicketDetailList)
                                {
                                    if (!isHeaderBind)
                                    {
                                        rowIndex++;
                                        childColIndex = 2;

                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_SaudaNumber"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_OilType"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Sku"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_BidQuantityPerMT"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_BidQuantityCases"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_BidPriceInRs"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_BidPricePerSku"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_BookingDate"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Plant"));
                                        isHeaderBind = true;
                                    }
                                    rowIndex++;
                                    childColIndex = 2;
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], tradeTicketSaudaOrderDetail.SaudhaNumber);
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], tradeTicketSaudaOrderDetail.Oiltype);
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], tradeTicketSaudaOrderDetail.Sku);
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], tradeTicketSaudaOrderDetail.BidQuantity.ToString());
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], tradeTicketSaudaOrderDetail.BidQuantityCase.ToString());
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], tradeTicketSaudaOrderDetail.BidPrice.ToString());
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], tradeTicketSaudaOrderDetail.BidPricePerSku.ToString());
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], tradeTicketSaudaOrderDetail.BookingDate != null ? tradeTicketSaudaOrderDetail.BookingDate.ToString() : string.Empty);
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], tradeTicketSaudaOrderDetail.PlantName);
                                }
                            }
                        }
                        worksheet.Cells.AutoFitColumns();
                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export Trade Ticket - Custom Export

        public async Task<ActionResult> ExportAllTradeTickets(TradeTicketSearchDto inputDto)
        {
            var finalResult = new JsonResult();
            try
            {
                string fileName = "TradeTickets_" + string.Format(Settings.ReportDateFormat, DateTime.Now.Date).ToUpper() + ".xlsx";
                bool isHeaderBind = false;

                var tradeTicketDetails = await _reverseAuctionClient.ExportAllTradeTickets(inputDto);
                if (tradeTicketDetails != null && tradeTicketDetails.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        var rowIndex = 7;
                        var colIndex = 1;

                        #region Header

                        worksheet.Cells["A1:M1"].Merge = true;
                        worksheet.Cells["A1:M1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:M1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:M1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:M1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["A3"].Value = "From date";
                        worksheet.Cells["A4"].Value = "To Date";

                        worksheet.Cells["B2"].Value = "Trade Ticket";
                        worksheet.Cells["B3"].Value = string.Format(Settings.ReportDateFormat, inputDto.FromDate);
                        worksheet.Cells["B4"].Value = string.Format(Settings.ReportDateFormat, inputDto.ToDate);

                        for (int i = 2; i <= 4; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        #endregion

                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ContractTypes"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_MaterialType"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_BookingType"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TradeTicketNumber"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ContractDate"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TotalQuantityInMT"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SaudaBookedQuantityInMT"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OpenQuantityInMT"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Plant"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OilType"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_SAPCreationDate"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_RatePerMT"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidFrom"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ValidTo"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_OtherElement"));

                        foreach (var tradeTicket in tradeTicketDetails)
                        {
                            isHeaderBind = false;
                            rowIndex++;
                            colIndex = 1;

                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.ContractType);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.MaterialType);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.BookingType);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.TradeTicketNumber);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.ContractDate != null ? tradeTicket.ContractDate.ToString(Settings.DateFormat) : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.ContractQuantity.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.SaudaBookedQuantity.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.OpenQty.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.PlantName);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.TradeTicketOilTypes);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.SAPCreationDate != null ? tradeTicket.SAPCreationDate.ToString(Settings.DateFormat) : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.RatePerMT.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.ValidFrom != null ? tradeTicket.ValidFrom.Value.ToString(Settings.DateFormat) : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.ValidTo != null ? tradeTicket.ValidTo.Value.ToString(Settings.DateFormat) : string.Empty);
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], tradeTicket.OtherElement.ToString());
                        }
                        worksheet.Cells.AutoFitColumns();
                        return SaveExcelFileToPath(package, fileName);
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SAP Data Sync

        public ActionResult SAPDataSync()
        {
            return View();
        }

        //public ActionResult GetSAPDataSyncListForDropdown([DataSourceRequest] DataSourceRequest request)
        //{
        //    var result = _reverseAuctionClient.GetSAPDataSyncListForDropdown();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetSyncTypeForDropdown([DataSourceRequest] DataSourceRequest request)
        {
            var result = _reverseAuctionClient.GetSyncTypeForDropdown();
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult SAPSyncData(SAPDataSyncInputDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            inputDto = _reverseAuctionClient.SAPSyncData(inputDto);
            inputDto.PostStatus = true;
            return Json(inputDto, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public async Task<ActionResult> TradeTicketSaudaUnMapping(long saudaId)
        {
            var inputDto = new TradeTicketSaudaUnMappingDto { SaudaId = saudaId, LoginUserId = UserId };
            var result = await _reverseAuctionClient.TradeTicketSaudaUnMapping(inputDto);
            if (result.PostStatus)
                result.PostMessage = Helper.GetResourceString("msg_TradeticketSaudaUnMappedSuccess");
            else
                result.PostMessage = Helper.GetResourceString("msg_TradeticketSaudaUnMappedError");

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult BiddingWindowStatusWiseVolumeCount(BiddingWindowDashboardDto biddingWindowDashboardDto)
        {
            var resultList = _reverseAuctionClient.BiddingWindowStatusWiseVolumeCount(biddingWindowDashboardDto.BiddingWindowId);
            return Json(resultList);
        }

        public JsonResult BiddingWindowStatusSateWiseCount(BiddingWindowDashboardDto biddingWindowDashboardDto)
        {
            var resultList = _reverseAuctionClient.BiddingWindowStatusSateWiseCount(biddingWindowDashboardDto.BiddingWindowId, biddingWindowDashboardDto.StateId);
            return Json(resultList);
        }

        [NoCache]
        public async Task<JsonResult> SaudaDetailsById([DataSourceRequest] DataSourceRequest request, int SaudaId)
        {
            List<SaudaListDto> saudadetails = new List<SaudaListDto>();
            if (SaudaId > 0)
            {
                IdInputDto idInputDto = new IdInputDto { Id = SaudaId };
                SaudaListsDto result = await _reverseAuctionClient.GetSaudaDetails(idInputDto);

                if (result.SaudaList != null)
                {
                    saudadetails = result.SaudaList;
                    var resultList = saudadetails.ToDataSourceResult(request);
                    resultList.Total = saudadetails.Count;
                    return Json(resultList);
                }
            }
            return Json(saudadetails);
        }
    }
}