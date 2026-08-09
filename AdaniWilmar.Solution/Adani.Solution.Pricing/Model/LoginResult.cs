using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Model
{
   public class LoginResult
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AuthorizeOutputDto Authenticate { get; set; }

        public LoginResult()
        {
            Authenticate = new AuthorizeOutputDto();
            Username = ConfigurationManager.AppSettings["Username"];
            Password = ConfigurationManager.AppSettings["Password"];
        }
    }
}
