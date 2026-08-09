using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using GMCore.Helper;
using Adani.Solution.MVC.Models;
using Adani.Solution.MVC.ServiceClient;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.DTO.Enums;
using Adani.Solution.DTO;
using Newtonsoft.Json.Linq;
using System.Linq;
using OfficeOpenXml;
using System.IO;
using System.Web.Hosting;
using System.Drawing;
using Adani.Solution.DTO.Common;
using Kendo.Mvc;
using Adani.Solution.MVC.Common;
using System.Web;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class SupportController : BaseController
    {
        private readonly SupportClient _supportClient;

        public SupportController()
        {
            _supportClient = new SupportClient { ControllerDelegate = this };
        }

        #region Dropdown - Lookups

        public ActionResult GetIssueTypeListForDropdown([DataSourceRequest] DataSourceRequest request)
        {
            var result = _supportClient.GetIssueTypeListForDropdown();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSeverityListForDropdown([DataSourceRequest] DataSourceRequest request)
        {
            var result = _supportClient.GetSeverityListForDropdown();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetApplicationComponentModuleListForDropdown([DataSourceRequest] DataSourceRequest request)
        //{
        //    var result = _supportClient.GetModuleListForDropdown();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public async Task<ActionResult> GetFeatureListForDropdown([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
            var featureTypeList = await _supportClient.GetFeatureListForDropdown(inputDto);
            return Json(featureTypeList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSupportIssueStatusListForDropdown([DataSourceRequest] DataSourceRequest request)
        {
            var result = _supportClient.GetSupportIssueStatusListForDropdown();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> GetQueryFromListForDropdown([DataSourceRequest] DataSourceRequest request)
        {
            LoginUserIdDto inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
            var featureTypeList = await _supportClient.GetQueryFromListForDropdown(inputDto);
            return Json(featureTypeList, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetQueryFromListForDropdown([DataSourceRequest] DataSourceRequest request)
        //{
        //    var result = _supportClient.GetQueryFromListForDropdown();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetRaisedByListForDropdown([DataSourceRequest] DataSourceRequest request)
        {
            var result = _supportClient.GetRaisedByListForDropdown();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [AuthorizeClaims(Claims.ManageSupport)]
        public ActionResult IssueList()
        {
            return View();
        }

        public async Task<ActionResult> GetSupportIssueListAsync([DataSourceRequest] DataSourceRequest request, SupportFilterInputDto inputDto)
        {
            inputDto.DataSourceRequest = request;
            inputDto.LoginUserId = UserId;
            var result = await _supportClient.GetSupportIssueListAsync(inputDto);
            return Json(result);
        }

        public async Task<ActionResult> GetSupportIssueListWithCmtsAsync([DataSourceRequest] DataSourceRequest request, SupportFilterInputDto inputDto)
        {
            inputDto.DataSourceRequest = request;
            inputDto.LoginUserId = UserId;
            var result = await _supportClient.GetSupportIssueListWithCmtsAsync(inputDto);
            return Json(result);
        }

        /// <summary>
        /// Hierical grid for Issue ID
        /// </summary>
        /// <param name="request"></param> 
        /// <returns></returns>
        public async Task<ActionResult> GetIssueCommentsListAsync(int supportId, [DataSourceRequest] DataSourceRequest request, bool isToReturnInactiveData)
        {
            var zoneList = await _supportClient.GetIssueCommentsListAsync(supportId);
            var resultList = zoneList.ToDataSourceResult(request);
            return Json(resultList);
        }

        public async Task<ActionResult> IssueDetails()
        {
            var result = new IssueRegisterDto();
            if (Session["SupportId"] != null && UtilityHelper.IntTryToParse(Session["SupportId"].ToString()) > 0)
            {
                var inputDto = new IssueDetailInputDto { SupportId = UtilityHelper.IntTryToParse(Session["SupportId"].ToString()), UserId = this.UserId };
                result = await _supportClient.GetIssueDetailsBySupportId(inputDto);
            }
            return View(result);
        }

        public ActionResult IssueDetailsEdit(int supportId)
        {
            Session["SupportId"] = supportId;
            return RedirectToAction("IssueDetails", "Support");
        }

        public ActionResult IssueRegister()
        {
            var issueDto = new IssueRegisterDto();
            return View(issueDto);
        }

        [HttpPost]
        public async Task<ActionResult> IssueRegister(IssueRegisterDto inputDto, IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                var fileSizeResult = _supportClient.CheckImageSizeAndType(files);
                if (fileSizeResult.IsSuccess)
                {
                    inputDto.LoginUserId = UserId;
                    inputDto = await _supportClient.SaveSupportIssue(inputDto, files);
                    return View(inputDto);
                }
                else
                {
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = fileSizeResult.ErrorDto.Message;

                }
            }
            else
            {
                inputDto.PostStatus = false;
                inputDto.PostMessage = Helper.GetResourceString("msg_PleaseSelectAnyMedia");
            }
            
            return View(inputDto);
        }

        public KendoGridResult GridResultInputDto(DataSourceRequest request, bool isToReturnInactiveData)
        {
            request.Filters = Utility.ToFilterDescriptor(request.Filters);
            return new KendoGridResult() { LoginUserId = UserId, IsToReturnInactiveData = isToReturnInactiveData, DataSourceRequest = request };
        }

        [HttpPost]
        public async Task<JsonResult> UpdateIssueStatus(IssueStatusUpdateDto inputDto)
        {
            inputDto.ModifiedBy = UserId;
            inputDto = await _supportClient.UpdateIssueStatus(inputDto);
            return Json(inputDto, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ExportSupportIssues(SupportFilterInputDto inputDto)
        {
            var finalResult = new JsonResult();
            inputDto.LoginUserId = UserId;
            try
            {
                string fileName = "SupportIssues_" + string.Format(Settings.ReportDateFormat, DateTime.Now.Date).ToUpper() + ".xlsx";
                bool isHeaderBind = false;
                var resultList = await _supportClient.ExportSupportIssues(inputDto);

                if (resultList != null && resultList.Any())
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        var rowIndex = 5;
                        var colIndex = 1;
                        var childColIndex = 0;

                        #region Header

                        worksheet.Cells["A1:K1"].Merge = true;
                        worksheet.Cells["A1:K1"].Value = Settings.CompanyName;
                        worksheet.Cells["A1:K1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1:K1"].Style.Font.Bold = true;
                        worksheet.Cells["A1:K1"].Style.Font.Size = 16;

                        worksheet.Cells["A2"].Value = "Report Name";
                        worksheet.Cells["B2"].Value = "Support Issue Details";

                        for (int i = 2; i <= 4; i++)
                        {
                            worksheet.Cells["A" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells["A" + i].Style.Font.Bold = true;
                            worksheet.Cells["A" + i].Style.Font.Size = 12;

                            worksheet.Cells["B" + i + ":" + "F" + i].Merge = true;
                            worksheet.Cells["B" + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        }

                        #endregion

                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_CreatedDateTime"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Description"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_QueryFrom"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Feature"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Component"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IssueType"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Impact"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_IssueRaisedBy"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_StateOfUser"));
                        //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ApplicationComponent"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_Status"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_ResolutionDateTime"));
                        GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], @Helper.GetResourceString("lbl_TimeTakenForResolution"));

                        foreach (var item in resultList)
                        {
                            isHeaderBind = false;
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.CreatedDateTime.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Description.ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IssueFromDevice.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Feature.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Component.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Impact.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.IssueRaisedByUserName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.State.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.Status.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ResolvedDateTime.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.TimeTakenToResolve.ToString());

                            if (item.Comments != null && item.Comments.Any())
                            {
                                foreach (var comment in item.Comments)
                                {
                                    if (!isHeaderBind)
                                    {
                                        rowIndex++;
                                        childColIndex = 2;

                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_Comments"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_CommentedDate"));
                                        GetExcelTitle(worksheet.Cells[rowIndex, childColIndex++], @Helper.GetResourceString("lbl_CommentedBy"));
                                        isHeaderBind = true;
                                    }
                                    rowIndex++;
                                    childColIndex = 2;

                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], comment.Comments);
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], comment.CommentedDate.ToString());
                                    GetExcelContent(worksheet.Cells[rowIndex, childColIndex++], comment.CommentedBy);
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

    }
}