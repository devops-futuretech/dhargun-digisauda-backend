using Adani.Solution.MVC.Attributes;
using Adani.Solution.DTO.Enums;
using Adani.Solution.DTO;
using Adani.Solution.MVC.ServiceClient;
using System.Web.Mvc;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using GMCore.Helper;
using Adani.Solution.MVC.Helpers;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System;
using Adani.Solution.MVC.Common;
using Newtonsoft.Json;
using Adani.Solution.MVC.Models;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class SurveyUpdatesController : BaseController
    {
        private readonly SurveyUpdatesClient _updatesClient;
        private readonly LookupClient _lookupClient;
        private readonly MediaClient _mediaClient;

        public SurveyUpdatesController()
        {
            _updatesClient = new SurveyUpdatesClient { ControllerDelegate = this };
            _lookupClient = new LookupClient { ControllerDelegate = this };
            _mediaClient = new MediaClient { ControllerDelegate = this };
        }

        #region Question

        /// <summary>
        /// Method to get Question List page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageUpdate)]
        public ActionResult QuestionList()
        {
            return View();
        }

        /// <summary>
        /// Method to Get Question List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isToReturnInactiveData"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetQuestionListAsync([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData };
            var result = await _updatesClient.GetQuestionListAsync(loginUserIdDto);
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }

        /// <summary>
        /// Method to redirect Question add or update page
        /// </summary>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        public ActionResult QuestionEditRedirect(string QuestionId = "")
        {
            Session["QuestionId"] = QuestionId;
            return RedirectToAction("Question", "SurveyUpdates");
        }

        /// <summary>
        /// Method to get Question add or update page
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageUpdate)]
        public async Task<ActionResult> Question()
        {
            var result = new QuestionDto();
            if (Session["QuestionId"] != null && UtilityHelper.IntTryToParse(Session["QuestionId"].ToString()) > 0)
            {
                result = await _updatesClient.GetQuestionDetailsById(UtilityHelper.LongTryToParse(Session["QuestionId"].ToString()));
            }
            return View(result);
        }

        /// <summary>
        /// Method to  add or update Question
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> AddOrUpdateQuestion(QuestionDto inputDto)
        {
            inputDto.LoginUserId = UserId;
            var result = await _updatesClient.AddOrUpdateQuestion(inputDto);
            if (result.PostStatus)
            {
                TempData["SuccessMessage"] = result.PostMessage;
                return RedirectToAction("QuestionList", "SurveyUpdates");
            }
            return View("Question", result);
        }


        /// <summary>
        /// Method to redirect Question review
        /// </summary>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        public ActionResult QuestionSurveyRedirect(string questionId = "")
        {
            Session["QuestionSurveyId"] = questionId;
            return RedirectToAction("QuestionSurvey", "SurveyUpdates");
        }


        /// <summary>
        /// Method to get Question Survey
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> QuestionSurvey()
        {
            var result = new QuestionDto();
            if (Session["QuestionSurveyId"] != null && UtilityHelper.IntTryToParse(Session["QuestionSurveyId"].ToString()) > 0)
            {
                result = await _updatesClient.GetQuestionSurveyDetailsById(UtilityHelper.LongTryToParse(Session["QuestionSurveyId"].ToString()));
            }
            return View(result);
        }

        #endregion

        #region Bulletin
        /// <summary>
        /// Method to get method for BulletinList
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageUpdate)]
        public ActionResult BulletinList()
        {
            Session["BulletinId"] = null;
            return View();
        }

        /// <summary>
        /// Method to get Bulletin list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [NoCache]
        public async Task<ActionResult> GetBulletinListAsync([DataSourceRequest] DataSourceRequest request, int contentTypeId = 0)
        {
            var bulletinInputDto = new BulletinInputDto
            {
                ContentTypeId = contentTypeId,
                LoginUserId = UserId
            };
            IList<BulletinDto> result = new List<BulletinDto>();
            var totals = new long[1];
            if (request.Filters != null && request.Filters.Any())
            {
                result = Session["BulletinList"] != null ? (List<BulletinDto>)Session["BulletinList"] : result;
            }
            else
            {
                result = await _updatesClient.GetBulletinListAsync(request, totals, bulletinInputDto);
                Session["BulletinList"] = result;
            }
            var gridOutputList = result.ToDataSourceResult(request);
            //gridOutputList.Total = (int)totals[0];
            return Json(gridOutputList);
        }

        public ActionResult ManageBulletinEditRedirect(long? bulletinId)
        {
            Session["BulletinId"] = bulletinId;
            return RedirectToAction("Bulletin", "SurveyUpdates");
        }

        /// <summary>
        /// Method to get method for Bulletin
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageUpdate)]
        public async Task<ActionResult> Bulletin()
        {
            var result = new BulletinDto();
            if (Session["BulletinId"] != null && UtilityHelper.LongTryToParse(Session["BulletinId"].ToString()) > 0)
            {
                result = await _updatesClient.GetBulletinDetailsByIdAsync(UtilityHelper.IntTryToParse(Session["BulletinId"].ToString()), UserId);
                result.Content = Server.HtmlDecode(result.Content);
                result.IsEdit = true;
            }
            return View(result);
        }

        /// <summary>
        /// Method to post method for Bulletin
        /// </summary>
        /// <param name="BulletinViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> Bulletin(BulletinDto bulletinDto, IEnumerable<HttpPostedFileBase> files)
        {
            //ModelState.Clear();
            bulletinDto.LoginUserId = UserId;
            bulletinDto = Helper.SanitizeModel<BulletinDto>(bulletinDto);


            if (files == null && Session["imagefile"] != null)
            {
                files = Session["imagefile"] as IEnumerable<HttpPostedFileBase>;
            }

            if (files != null && files.Any())
            {
                var fileSizeResult = _mediaClient.CheckImageSizeType(files);
                if (fileSizeResult.IsSuccess)
                {
                    var folderName = bulletinDto.ContentTypeId > 0 ? Enum.GetName(typeof(ContentType), bulletinDto.ContentTypeId) : Enum.GetName(typeof(ContentType), (int)ContentType.LatestUpdate);

                    var mediaResult = _mediaClient.SaveMediaFile(files, folderName);
                    if (mediaResult != null && mediaResult.Count(_ => _.IsSuccess) == files.Count())
                    {
                        var result = await _updatesClient.AddOrUpdateBulletin(bulletinDto, mediaResult);
                        if (result.PostStatus)
                        {
                            Session["imagefile"] = null;
                            TempData["SuccessMessage"] = result.PostMessage;
                            return RedirectToAction("BulletinList", "SurveyUpdates");
                        }
                        if (files != null)
                        {
                            Session["imagefile"] = files;
                            var filesDetails = files.Select(x => new { name = x.FileName, extension = x.ContentType, size = x.ContentLength });
                            var filesJson = JsonConvert.SerializeObject(filesDetails);
                            bulletinDto.FileDetail = filesJson;
                        }

                        foreach (var item in mediaResult)
                            _mediaClient.DeleteFile(item.FileName, folderName);

                        bulletinDto.PostStatus = false;
                        bulletinDto.PostMessage = result.PostMessage;
                    }
                    else
                    {
                        bulletinDto.PostStatus = false;
                        bulletinDto.PostMessage = Helper.GetResourceString("msg_ImageUploadError");
                    }
                }
                else
                {
                    bulletinDto.PostStatus = false;
                    bulletinDto.PostMessage = Helper.GetResourceString("msg_PleaseSelectAnyOneImage");
                }
            }
            else
            {
                var result = await _updatesClient.AddOrUpdateBulletin(bulletinDto, null);
                if (result.PostStatus)
                {
                    TempData["SuccessMessage"] = result.PostMessage;
                    return RedirectToAction("BulletinList", "SurveyUpdates");
                }
                bulletinDto.PostStatus = false;
                bulletinDto.PostMessage = result.PostMessage;
            }
            bulletinDto.Content = Server.HtmlDecode(bulletinDto.Content);
            return View(bulletinDto);
        }

        /// <summary>
        /// Method to delete bulltin media
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> DeleteBulletinMediaAsync(string bulletinMediaId, string fileName, int contentTypeId)
        {
            var result = new BulletinDto();
            if (bulletinMediaId != null && UtilityHelper.IntTryToParse(bulletinMediaId) > 0)
            {
                result = await _updatesClient.DeleteBulletinMediaAsync(UtilityHelper.IntTryToParse(bulletinMediaId), UserId);
                if (result.PostStatus)
                {
                    var folderName = Enum.GetName(typeof(MediaType), contentTypeId);
                    _mediaClient.DeleteFile(fileName, folderName);
                    var successMessage = Helper.GetResourceString("msg_DeleteMediaSuccessful");
                    result.PostStatus = true;
                    result.PostMessage = successMessage;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get entity list
        /// </summary>
        /// <returns></returns>
        public JsonResult GetContentTypeList([DataSourceRequest] DataSourceRequest request)
        {
            var approveList = ((ContentType[])Enum.GetValues(typeof(ContentType))).Select(c => new EnumModel() { EntityTypeId = (int)c, Name = c.Description() }).ToList();
            return Json(approveList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Feedback
        /// <summary>
        /// Method to Get Sauda limit
        /// </summary>
        /// <returns></returns>
        [AuthorizeClaims(Claims.ManageUpdate)]
        public ActionResult Feedback()
        {
            return View();
        }

        public async Task<ActionResult> GetFeedbackTypeddl([DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            var loginUserIdDto = new LoginUserIdDto();
            loginUserIdDto.IsToReturnInactiveData = isToReturnInactiveData;
            loginUserIdDto.LoginUserId = UserId;
            var result = await _updatesClient.GetFeedbackTypeddl(loginUserIdDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Get Feedback List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetFeedbackListAsync([DataSourceRequest] DataSourceRequest request, FeedbackInputDto feedbackInputDto)
        {
            var result = new List<FeedbackRequestDto>();
            if (feedbackInputDto != null && feedbackInputDto.CreatedDate != null)
            {
                feedbackInputDto.LoginUserId = UserId;
                result = await _updatesClient.GetFeedbackListAsync(feedbackInputDto);
                //if (result != null && result[0] != null && result[0].PostStatus == false)
                //{
                //    ModelState.AddModelError("Feedback", result[0].PostMessage);
                //    return Json(result.AsQueryable().ToDataSourceResult(request, ModelState));
                //}

            }
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
        }
        #endregion
    }
}