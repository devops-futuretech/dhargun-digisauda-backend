
namespace Adani.Solution.DTO
{
    public class ResetPasswordDto : UserIdDto
    {
        public string NewPassword { get; set; }
        public string OtpNumber { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsResendOTP { get; set; }
        public bool IsPageLoad { get; set; }
    }
}
