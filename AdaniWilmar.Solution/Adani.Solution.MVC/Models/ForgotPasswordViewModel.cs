using Adani.Solution.DTO;

namespace Adani.Solution.MVC.Models
{
    public class ForgotPasswordViewModel : ResetPasswordDto
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long VerticalId { get; set; }
    }
}