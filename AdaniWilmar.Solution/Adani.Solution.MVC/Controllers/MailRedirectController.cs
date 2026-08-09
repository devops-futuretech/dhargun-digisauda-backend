using Adani.Solution.DTO;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.ServiceClient;
using GMCore.Helper;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Adani.Solution.MVC.Controllers
{
    [NoCache]
    public class MailRedirectController : BaseController
    {
        private const string ServiceName = "Mail Redirect Controller";
        private readonly ILogger _logger = Logging.GetLogger("MailRedirectController");
        private string _methodName;
        private readonly MailRedirectClient _mailRedirectClient;

        public MailRedirectController()
        {
            _mailRedirectClient = new MailRedirectClient { ControllerDelegate = this };
        }

        public async Task<ActionResult> ApproveSaudaCounterBid(string saudaOrderEncryptedId = "")
        {
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SaudaOrderDetails();
            try
            {                
                if (!string.IsNullOrEmpty(saudaOrderEncryptedId))
                {
                    var orderEncryptedId = EncryptDecryptHelper.Encrypt("7", SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    saudaOrderEncryptedId = saudaOrderEncryptedId.Replace(" ", "+");
                    var decryptedSaudaOrderId = UtilityHelper.IntTryToParse(EncryptDecryptHelper.Decrypt(saudaOrderEncryptedId, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey));
                    var counterBidDto = new SaudaDetailInputDto { SaudaOrderId = decryptedSaudaOrderId, UserId = this.UserId };
                    result = await _mailRedirectClient.GetSaudaCounterBidDetails(counterBidDto);
                }               
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.PostStatus = false;
                result.PostMessage = exception.Message;
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [ValidateInput(false)]
        public async Task<JsonResult> ApproveSaudaCounterBid(CounterBidInputDto inputDto)
        {
            var result = await _mailRedirectClient.ApproveCounterBid(inputDto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }        
        
    }
}