using Adani.Solution.DTO;

namespace Adani.Solution.MVC.Models
{
    public class LoginViewModel
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long VerticalId { get; set; }
        public long HeadquartersId { get; set; }
        public long OrganizationReportingToId { get; set; }
        public AuthorizeOutputDto Authenticate { get; set; }

        public LoginViewModel()
        {
            Authenticate = new AuthorizeOutputDto();
        }
    }
}