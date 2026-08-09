
namespace Adani.Solution.DTO
{
    public class CounterBidInputDto : LoginUserIdDto
    {
        public int SaudaOrderId { get; set; }
        public bool IsAccept { get; set; }
    }

    public class CounterBiddingInputDto
    {
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
        public long OilTypeId { get; set; }
        public long BiddingWindowId { get; set; }
        public long SkuId { get; set; }
        public long IncotermId { get; set; }
        public string DealerMobileNumber { get; set; }
    }

    public class CounterBidPushNotificationDto
    {
        public long ToUserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string PushTokenKey { get; set; }
        public int RegistrationTypeId { get; set; }        
        public bool IsLogOut { get; set; }

        public int SaudaBiddingCartId { get; set; }        
    }
}
