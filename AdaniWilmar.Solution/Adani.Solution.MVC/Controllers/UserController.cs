using System.Web.Mvc;
using GMCore.Helper;
using Newtonsoft.Json;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Models;
using System.Threading.Tasks;
using Adani.Solution.MVC.ServiceClient;
using System.Web;
using System;
using Adani.Solution.DTO.Enums;
using System.Web.Security;
using CaptchaMvc.HtmlHelpers;
using System.Linq;
using Adani.Solution.DTO;
using Kendo.Mvc.UI;
using System.Collections.Generic;

namespace Adani.Solution.MVC.Controllers
{
    [NoCache]
    public class UserController : BaseController
    {
        private readonly UserClient _userClient;

        public UserController()
        {
            _userClient = new UserClient { ControllerDelegate = this };
        }

        /// <summary>
        /// Method to get login action
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            Session["UserClaims"] = null;
            ViewBag.Verticals = _userClient.GetAllVerticals();
            var result = new LoginViewModel();
            return View(result);
        }

        /// <summary>
        /// Method to post login action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            loginViewModel = Helper.SanitizeModel<LoginViewModel>(loginViewModel);
            if (this.IsCaptchaValid(Helper.GetResourceString("msg_CaptchaIsNotValid")))
            {
                loginViewModel = await _userClient.ValidateUserAsync(loginViewModel);
                if (loginViewModel.PostStatus)
                {
                    if (Session["UserClaims"] == null)
                    {
                        var claimDetails = _userClient.GetClaimDetailsByIdAsync(loginViewModel.Authenticate.UserId, loginViewModel.Authenticate.LoginToken).Result;
                        if (claimDetails != null && claimDetails.Where(_ => _.IsApplied).Any())
                        {
                            Session["UserClaims"] = claimDetails;
                        }
                    }

                    CreateCookie(loginViewModel, false);
                    TempData["SuccessMessage"] = loginViewModel.PostMessage;
                    return RedirectToLocal();

                }
            }
            else
            {
                loginViewModel.PostMessage = Helper.GetResourceString("msg_CaptchaVerificationFailed");
                loginViewModel.PostStatus = false;
            }
            return View(loginViewModel);
        }

        /// <summary>
        /// Method to get forgot password action
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPassword()
        {
            Session["UserClaims"] = null;
            Session["ForgotPasswordOtpCount"] = null;
            Session["ResendOtpCount"] = null;

            var result = new ForgotPasswordViewModel();
            return View(result);
        }

        /// <summary>
        /// Method to post forgot password action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            forgotPasswordViewModel = Helper.SanitizeModel<ForgotPasswordViewModel>(forgotPasswordViewModel);
            if (this.IsCaptchaValid(Helper.GetResourceString("msg_CaptchaIsNotValid")))
            {
                forgotPasswordViewModel = await _userClient.ChangePasswordAsync(forgotPasswordViewModel);
                if (forgotPasswordViewModel.PostStatus)
                {
                    Session["UserId"] = forgotPasswordViewModel.UserId;
                    Session["UserName"] = forgotPasswordViewModel.Username;
                    return RedirectToAction("ForgotPasswordOtp", "User");
                }
            }
            else
            {
                forgotPasswordViewModel.PostMessage = Helper.GetResourceString("msg_CaptchaVerificationFailed");
                forgotPasswordViewModel.PostStatus = false;
            }
            return View(forgotPasswordViewModel);
        }


        /// <summary>
        /// Method to get forgot password OTP action
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPasswordOtp()
        {
            long userId = 0;
            string userName = "";
            if (Session["UserId"] != null)
            {
                userId = UtilityHelper.LongTryToParse(Session["UserId"].ToString());
                userName = Session["UserName"].ToString();
                Session["UserId"] = null;
                Session["UserName"] = "";

            }

            var result = new ForgotPasswordViewModel()
            {
                UserId = userId,
                Username = userName,
                Message = Helper.GetResourceString("msg_OtpHasBeenSentToMobile"),
                IsPageLoad = true
            };
            return View(result);
        }

        /// <summary>
        /// Method to post forgot password OTP action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<ActionResult> ForgotPasswordOtp(ForgotPasswordViewModel resetPasswordDto)
        {
            resetPasswordDto = Helper.SanitizeModel<ForgotPasswordViewModel>(resetPasswordDto);

            if (resetPasswordDto.IsResendOTP)
            {
                if (this.IsCaptchaValid(Helper.GetResourceString("msg_CaptchaIsNotValid")))
                {
                    var resendOtpCount = Session["ResendOtpCount"] != null ? UtilityHelper.IntTryToParse(Session["ResendOtpCount"].ToString()) : 0;
                    if (resendOtpCount < Settings.VerifyOtpHit)
                    {
                        Session["ResendOtpCount"] = resendOtpCount + 1;
                        var resendResult = await _userClient.ResendOtpAsync(resetPasswordDto.UserId);
                        resetPasswordDto.PostStatus = resendResult.IsSuccess;
                        resetPasswordDto.PostMessage = resendResult.ErrorDto.Message;
                    }
                    else
                    {
                        resetPasswordDto.PostStatus = false;
                        resetPasswordDto.PostMessage = Helper.GetResourceString("msg_ResendOtpExceedLimitForgotPassword");
                    }
                }
                else
                {
                    resetPasswordDto.PostMessage = Helper.GetResourceString("msg_CaptchaVerificationFailed");
                    resetPasswordDto.PostStatus = false;
                }
            }
            else
            {
                var forgetPasswordOtpCount = Session["ForgotPasswordOtpCount"] != null ? UtilityHelper.IntTryToParse(Session["ForgotPasswordOtpCount"].ToString()) : 0;
                if (forgetPasswordOtpCount < Settings.VerifyOtpHit)
                {
                    Session["ForgotPasswordOtpCount"] = forgetPasswordOtpCount + 1;
                    resetPasswordDto = await _userClient.ChangePasswordOtpVerificationAsync(resetPasswordDto);
                    if (resetPasswordDto.PostStatus)
                    {
                        TempData["SuccessMessage"] = resetPasswordDto.PostMessage;
                        return RedirectToAction("SuccessMessage", "User");
                    }
                }
                else
                {
                    resetPasswordDto.PostStatus = false;
                    resetPasswordDto.PostMessage = Helper.GetResourceFor("msg_ForgotPasswordOTPLimitExceed");
                }
            }

            return View(resetPasswordDto);
        }

        public ActionResult SuccessMessage()
        {
            return View();
        }

        /// <summary>
        /// Method to sign out
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            ClearData();
            return RedirectToAction("Login", "User");
        }

        private void CreateCookie(LoginViewModel loginDto, bool rememberMe)
        {
            //if (loginDto.Authenticate != null && string.IsNullOrEmpty(loginDto.Authenticate.MobileNumber))
            //    loginDto.Authenticate.MobileNumber = loginDto.Authenticate.EncryptedUserId;

            var authCookie = CreateAuthCookie(loginDto.Authenticate.UserId.ToString(), loginDto.Authenticate);
            Response.AppendCookie(authCookie);
            ControllerContext.HttpContext.User = Settings.GetIdentity(loginDto.Authenticate);
            if (rememberMe)
            {
                var userCookie = new HttpCookie(Settings.CookiePrefix + Settings.CookieUsername)
                {
                    Value = loginDto.Authenticate.UserId.ToString(),
                    Expires = DateTime.Now.AddYears(1)
                };
                ControllerContext.HttpContext.Response.Cookies.Add(userCookie);
            }
            else
            {
                HttpCookie savedUser = ControllerContext.HttpContext.Request.Cookies[Settings.CookiePrefix + Settings.CookieUsername];
                if (savedUser != null)
                {
                    savedUser.Expires = DateTime.Now.AddDays(-1);
                    ControllerContext.HttpContext.Response.Cookies.Add(savedUser);
                }
            }
        }

        public ActionResult RedirectToLocal(string returnUrl = "")
        {

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (User.Identity.IsAuthenticated || UserId > 0)
            {


                bool iAdmin = false, isSystem = false, isZonalHead = false, iDealer = false;
                var userProfile = Settings.GetStaticUserProfile(Request);

                if (userProfile != null && !string.IsNullOrWhiteSpace(userProfile.RoleId))
                {
                    var roles = userProfile.RoleId.Split(',').Select(int.Parse);
                    iAdmin = roles.Any(x => x == (int)Role.Admin);
                    //isSystem = roles.Any(x => x == (int)Role.System);
                    isZonalHead = roles.Any(x => x == (int)Role.ZonalTrader);
                    iDealer = roles.Any(x => x == (int)Role.Dealer);
                }

                //if (iAdmin || IsAdmin)
                //{
                //    return RedirectToAction("UpdateRole", "Role");
                //}
                //if (iDealer || IsDealer)
                //{
                //    return RedirectToAction("UpdateRole", "Role");
                //}
                //if (isZonalHead || IsZonalHead)
                //{
                //    return RedirectToAction("CompetitorAnalysisList", "Sauda");
                //}

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Logout", "User");
        }

        private void ClearData()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Method to get Get Vertical List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [NoCache]
        public async Task<JsonResult> GetVerticalListBasedOnUserAsync([DataSourceRequest] DataSourceRequest request, string username)
        {
            if (string.IsNullOrEmpty(username)) return Json(new List<DropDownDto>(), JsonRequestBehavior.AllowGet);
            var result = await _userClient.GetVerticalListBasedOnUserAsync(username);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}