using Adani.Solution.DTO;
using Adani.Solution.DTO.Enums;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.ServiceClient;
using GMCore.Helper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Adani.Solution.MVC.Controllers
{

    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class QPSDiscountController : BaseController
    {
        private readonly QPSSchemeDiscountClient _qpsschemediscountClient;
        private readonly LookupClient _lookupClient;

        public QPSDiscountController()
        {
            _qpsschemediscountClient = new QPSSchemeDiscountClient { ControllerDelegate = this };
            _lookupClient = new LookupClient { ControllerDelegate = this };
        }

       
        [AuthorizeClaims(Claims.QPSDiscount, Claims.ViewMaster)]
        public ActionResult QPSDiscountList()
        {
            return View();
        }

        public async Task<ActionResult> QPSDiscountAddorUpdate()
        {
            var inputDto = new QPSSchemeDiscountDto();
            //var QPSDiscountId = Session["QPSDiscountId"];
            if (!String.IsNullOrEmpty(Session["QPSDiscountId"].ToString()))
            {
                inputDto.EncryptedId = Session["QPSDiscountId"].ToString();
                //inputDto = await _lookupClient.GetQpsDiscountById(id);
               
                inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);

                inputDto.Id = UtilityHelper.IntTryToParse(decryptedId);
                inputDto = await _qpsschemediscountClient.GetQpsDiscountById(inputDto);
            }
            return View(inputDto);
        }
        public async Task<ActionResult> GetQpsDiscountListForGridAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _qpsschemediscountClient.QpsListAsync();
            var resultList = result.ToDataSourceResult(request);
            return Json(resultList);
            //return Json(await _qpsschemediscountClient.GetKendoGridDataAsync<QPSSchemeDiscountDto>(GridResultInputDto(request, true), ApiUrl.WebApiUrlGetQPSDiscountListWithPagination));
        }
        public ActionResult QPSAddRedirect(string EncryptedId = "")
        {
            Session["QPSDiscountId"] = EncryptedId;
            return RedirectToAction("QPSDiscountAddorUpdate", "QPSDiscount");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public async Task<ActionResult> QpsAddOrUpdate(QPSSchemeDiscountDto qPSSchemeDiscountDto, string QPSSlabDetails = "")
        {
            qPSSchemeDiscountDto.QPSSlabDetails = GMCore.Helper.JsonHelper.ConvertJSonToObject<List<QPSSlabDetailsDto>>(QPSSlabDetails);
            qPSSchemeDiscountDto.LoginUserId = UserId;
            var result = await _qpsschemediscountClient.QpsAddOrUpdate(qPSSchemeDiscountDto);         
            return Json(result, JsonRequestBehavior.AllowGet);
            //if (result.PostStatus)
            //{
            //    TempData["SuccessMessage"] = result.PostMessage;
            //    return Json(new { success = true, message = result.PostMessage }, JsonRequestBehavior.AllowGet);
            //    //return RedirectToAction("QPSDiscountList", "QPSDiscount");
            //}
            //else
            //{
            //    qPSSchemeDiscountDto.PostStatus = false;
            //    qPSSchemeDiscountDto.PostMessage = result.PostMessage;
            //}
            //return Json(new { success = false, message = result.PostMessage, data = qPSSchemeDiscountDto }, JsonRequestBehavior.AllowGet);
            //return View("QPSDiscountAddorUpdate", qPSSchemeDiscountDto);
        }

        private ExcelRange GetExcelContent(ExcelRange cell, string text, int align = 0, int level = 0)
        {
            cell.Value = text ?? string.Empty;

            cell.Style.Border.Top.Style =
                cell.Style.Border.Left.Style =
                    cell.Style.Border.Right.Style = cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            if (align == 1)//align right
            {
                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            }


            if (level > 0)
            {
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.LightSeaGreen);
            }
            return cell;
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

        public async Task<ActionResult> ExportQPSSchemeDiscount(LoginUserIdDto inputDto)
        {
            string guidFileName = "";
            string fileName = "";
            try
            {
                inputDto = new LoginUserIdDto { IsToReturnInactiveData = true, LoginUserId = UserId };
                string serverFoloderPath = HostingEnvironment.MapPath("~/FinalPriceDownload/");
                List<QPSSchemeDiscountDto> resultList = new List<QPSSchemeDiscountDto>();
                resultList = await _qpsschemediscountClient.ExportQPSSchemeDiscount(inputDto);

                DateTime currentDate = DateTime.Now;
                fileName = "Division_" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";
                guidFileName = $"{Guid.NewGuid()}.xlsx";

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

                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Oil Type");
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Zone");
                    //GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "State");
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "Start Date");
                    GetExcelTitle(worksheet.Cells[rowIndex, colIndex++], "End Date");

                    if (resultList != null && resultList.Any())
                    {
                        foreach (var item in resultList)
                        {
                            rowIndex++;
                            colIndex = 1;
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.OilTypeName.ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.ZoneName.ToString());
                            //GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.StateName.ToString());
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.StartDate.ToString("dd-MM-yyyy"));
                            GetExcelContent(worksheet.Cells[rowIndex, colIndex++], item.EndDate.ToString("dd-MM-yyyy"));
                        }
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
                    //this.Response.Headers.Clear();
                    string savePath = Path.Combine(serverFoloderPath, guidFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        System.IO.File.Delete(savePath);
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                    else
                    {
                        using (Stream stream = System.IO.File.Create(savePath))
                        {
                            package.SaveAs(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                //result.Message = "Excel Error" + ex;
            }

            return Json(new { FileGuid = guidFileName, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }
      
    }
}