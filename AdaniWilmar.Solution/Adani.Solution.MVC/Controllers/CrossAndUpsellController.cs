using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.MVC.Attributes;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.ServiceClient;
using GMCore.Logger;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Windows.Input;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class CrossAndUpsellController : BaseController
    {
        private readonly ServiceClient.CrossAndUpsellClient _crossAndUpsellClient;
        private const string ControllerName = "CrossAndUpsellController";
        private readonly ILogger _logger = Logging.GetLogger(ControllerName);
        private string _methodName;
        public CrossAndUpsellController() 
        {
            _crossAndUpsellClient = new ServiceClient.CrossAndUpsellClient { ControllerDelegate = this };       
        }

        public ActionResult CrossAndUpsellList()
        {
            Session["SaudaConditonalBookingEncryptedId"] = null;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> AddCrossAndUpsell()
        {
            CrossAndUpsellConfigurationDto model = new CrossAndUpsellConfigurationDto();
            SuadaConditionalBookingInputDto inputModel = new SuadaConditionalBookingInputDto();
            inputModel.LoginUserId = UserId;

            if (Session["SaudaConditonalBookingEncryptedId"] != null && !string.IsNullOrEmpty(Session["SaudaConditonalBookingEncryptedId"].ToString()))
            {
                inputModel.EncryptedId = Session["SaudaConditonalBookingEncryptedId"].ToString();
                model = await _crossAndUpsellClient.GetSaudaConditionalBokkingConfigurationDetails(inputModel);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<JsonResult> AddCrossAndUpsell(CrossAndUpsellConfigurationDto InputModel,string SkuBookingCombinationString)
        {
            _methodName = "AddCrossAndUpsell";
            ResultDto result = new ResultDto();

            if(!string.IsNullOrEmpty(SkuBookingCombinationString))
            {
                InputModel.LoginUserId = UserId;
                InputModel.SkuBookingCombinationList = JsonConvert.DeserializeObject<List<SaudaConditionalBookingSkuDto>>(SkuBookingCombinationString);
            }

            if(ModelState.IsValid)
            {
               result =  await _crossAndUpsellClient.AddAndUpdateCrossAndUpsellConfiguration(InputModel);

               if(result.IsSuccess)
               {
                    TempData["SuccessMessage"] = result.SuccessDto.Message;
               }
            }

            return Json(result);
        }

        [OutputCache(Duration = 10)]
        public async Task<JsonResult> GetCrossAndUpsellList([DataSourceRequest] DataSourceRequest request)
        {
           SuadaConditionalBookingInputDto inputDto = new SuadaConditionalBookingInputDto { LoginUserId = UserId }; 
           var result = await _crossAndUpsellClient.GetSaudaConditionalBokkingConfigurationList(inputDto);
           return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCrossAndUpsellSkusList([DataSourceRequest] DataSourceRequest request,long Id)
        {
            SuadaConditionalBookingInputDto inputDto = new SuadaConditionalBookingInputDto { LoginUserId = UserId,Id = Id };
            var result = await _crossAndUpsellClient.GetSaudaConditionalBokkingConfigurationSkusList(inputDto);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSaudaConditionalBookingRedirect(string EncryptedId)
        {
            if(!string.IsNullOrEmpty(EncryptedId))
            {
                Session["SaudaConditonalBookingEncryptedId"] = EncryptedId;
            }

            return RedirectToAction("AddCrossAndUpsell");
        }

        [HttpPost]
        public async Task<ActionResult> ExportCrossAndUpsellConfiguration()
        {
            DateTime currentDate = DateTime.Now;
            string fileName = "CrossAndUpsellConfiguration" + string.Format("{0:dd-MMM-yyyy}", currentDate) + "-" + string.Format("{0:hh:mm tt}", currentDate) + ".xlsx";

            try
            {
                var resultList = await _crossAndUpsellClient.GetSaudaConditionalBokkingConfigurationListForReport(UserId);

                if (resultList.IsAny())
                {
                    using (var package = new ExcelPackage())
                    {
                        var ws = package.Workbook.Worksheets.Add("CrossAndUpsellConfiguration");

                        ws.Cells["A1:F1"].Merge = true;
                        ws.Cells["A1:F1"].Value = Settings.CompanyName;
                        ws.Cells["A1:F1"].Style.Font.Size = 14;
                        ws.Cells["A1:F1"].Style.Font.Name = "Calibri";
                        ws.Cells["A1:F1"].Style.Font.Bold = true;
                        ws.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells["A2:B2"].Merge = true;
                        ws.Cells["A2:B2"].Value = "Report Name";
                        ws.Cells["A2:B2"].Style.Font.Size = 12;
                        ws.Cells["A2:B2"].Style.Font.Name = "Calibri";
                        ws.Cells["A2:B2"].Style.Font.Bold = true;

                        ws.Cells["C2:F2"].Merge = true;
                        ws.Cells["C2:F2"].Value = "Cross-Selling And Upselling List";
                        ws.Cells["C2:F2"].Style.Font.Size = 12;
                        ws.Cells["C2:F2"].Style.Font.Name = "Calibri";
                        ws.Cells["C2:F2"].Style.Font.Bold = true;

                        ws.Cells["A4:AZ4"].Style.Font.Size = 12;
                        ws.Cells["A4:AZ4"].Style.Font.Name = "Calibri";
                        ws.Cells["A4:AZ4"].Style.Font.Bold = true;
                        ws.Cells["A4"].LoadFromCollection(resultList, true);
                        ws.Cells.AutoFitColumns();

                        return SaveExcelFileToPath(package, fileName);
                    }
                }

                return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error($" Controller Name {_methodName} Exception {ex.Message}");
                return Json(new { FileGuid = "", FileName = "" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}