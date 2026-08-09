using Adani.Solution.MVC.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adani.Solution.MVC.ServiceClient;
using System.Threading.Tasks;
using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;

namespace Adani.Solution.MVC.Controllers
{
    [TokenAuthorize]
    [CustomRedirect]
    [NoCache]
    public class HomeController : BaseController
    {
        private readonly ServiceClient.ServiceClient _serviceClient;

        public HomeController()
        {
            _serviceClient = new ServiceClient.ServiceClient { ControllerDelegate = this };
        }

        public async Task<ActionResult> Index()
        {
            LoginUserIdDto loginUserIdDto = new LoginUserIdDto { LoginUserId = UserId };

            var result = new DashboardDetailsDto();
            var scheduledTimeString = ConfigHelper.DashboardCardAPITime;
            DateTime scheduledDateTime;
            DateTime currentTime = DateTime.Now;
            scheduledDateTime = currentTime;
            if (DateTime.TryParseExact(scheduledTimeString, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime scheduledTime))
            {
                scheduledDateTime = scheduledTime;
            }
            if (ConfigHelper.ContractDashboard && currentTime <= scheduledDateTime)
            {
                var todayContract = await _serviceClient.GetDashboardDetails(new LoginUserIdDto { LoginUserId = UserId, IntercomId = (int)DTO.Enums.DashboardOption.TodayContract });
                if (todayContract != null)
                {
                    result.TodayContract = Math.Round(todayContract.TodayContract, 2);
                }
            }
            else
            {
                result.TodayContract = 0;
            }

            if (ConfigHelper.SalesOrderDashboard && currentTime <= scheduledDateTime)
            {
                var todaySalesOrder = await _serviceClient.GetDashboardDetails(new LoginUserIdDto { LoginUserId = UserId, IntercomId = (int)DTO.Enums.DashboardOption.TodaySalesOrder });
                if (todaySalesOrder != null)
                {
                    result.TodaySalesOrder = Math.Round(todaySalesOrder.TodaySalesOrder, 2);
                }
            }
            else
            {
                result.TodaySalesOrder = 0;
            }

            if (ConfigHelper.DueDashboard && currentTime <= scheduledDateTime)
            {
                var due = await _serviceClient.GetDashboardDetails(new LoginUserIdDto { LoginUserId = UserId, IntercomId = (int)DTO.Enums.DashboardOption.Due });

                if (due != null)
                {
                    result.OverDue = Math.Round(due.OverDue, 2);
                    result.TomorrowDue = Math.Round(due.TomorrowDue, 2);
                }
            }
            else
            {
                result.OverDue = 0;
                result.TomorrowDue = 0;
            }

            try
            {
                if (currentTime <= scheduledDateTime)
                {
                    var googleAnalyticsData = await _serviceClient.GetGoogleAnalyticsDataAsync(loginUserIdDto);
                    if (googleAnalyticsData != null)
                    {
                        result.GoogleAnalyticsData = googleAnalyticsData;
                    }
                }
                else
                {
                    result.GoogleAnalyticsData = new GoogleAnalyticsDataDto()
                    {
                        TotalEmployeesLoggedIn = 0,
                        TotalLoginsByDistributor = 0,
                        TotalLoginsBySales = 0,
                        ActiveUserCount = 0
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                // Log the error, etc.
            }

            //var todayInvoice = await _serviceClient.GetDashboardDetails(new LoginUserIdDto { LoginUserId = UserId, IntercomId = (int)DTO.Enums.DashboardOption.TodayInvoice });
            //if (todayInvoice != null)
            //{
            //    result.TodayInvoice = Math.Round(todayInvoice.TodayInvoice, 2);
            //}

            return View(result);
        }




        public ActionResult SyncSauda()
        {
            return View();
        }

        public async Task<ActionResult> CallGetHoldSkuinSauda()
        {
            string[] args = { "IsGetHoldSkuinSauda" };
            //Adani.Solution.API.Console.Program.Main(args);
            var inputDto = new LoginUserIdDto
            {
                LoginUserId = UserId
            };
            await _serviceClient.CallServiceNotification(inputDto, ApiUrl.WebApiUrlGetCounterBidNotification);
            return RedirectToAction("SyncSauda");
        }

        public async Task<ActionResult> CallSendLatestSaudasStatusNotification()
        {
            string[] args = { "IsSendLatestSaudasStatusNotification" };
            //Adani.Solution.API.Console.Program.Main(args);
            var inputDto = new LoginUserIdDto
            {
                LoginUserId = UserId
            };
            await _serviceClient.CallServiceNotification(inputDto, ApiUrl.WebApiUrlSendRABookingStatus);
            return RedirectToAction("SyncSauda");
        }

        public async Task<ActionResult> CallRejectSaudaOrdersInHold()
        {
            string[] args = { "IsRejectSaudaOrdersInHold" };
            //Adani.Solution.API.Console.Program.Main(args);
            var inputDto = new LoginUserIdDto
            {
                LoginUserId = UserId
            };
            await _serviceClient.CallServiceNotification(inputDto, ApiUrl.WebApiUrlUpdateHoldOrderToReject);
            return RedirectToAction("SyncSauda");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        //[AuthorizeRoles(Role.Admin)]
        public async Task<ActionResult> GoogleAnalyticsReport()
        {
            var inputDto = new LoginUserIdDto
            {
                LoginUserId = UserId
            };
            GoogleAnalyticsDataDto data = new GoogleAnalyticsDataDto();
            data = await _serviceClient.GetGoogleAnalyticsDataAsync(inputDto);
            return View(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetGoogleAnalyticsData()
        {
            GoogleAnalyticsDataDto analyticsData = new GoogleAnalyticsDataDto();
            var inputDto = new LoginUserIdDto
            {
                LoginUserId = UserId
            };
            try
            {
                analyticsData = await _serviceClient.GetGoogleAnalyticsDataAsync(inputDto);
                return Json(analyticsData);
            }
            catch (Exception ex)
            {
                return Json(analyticsData);
            }
        }


    }
}