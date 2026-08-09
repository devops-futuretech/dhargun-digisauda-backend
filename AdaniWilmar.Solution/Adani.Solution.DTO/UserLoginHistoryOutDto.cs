using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserLoginHistoryOutDto
    {
        [DisplayName("Distributor Name")]
        public string Name { get; set; }
        public string LoginDate { get; set; }
        [DisplayName("Initial Login Time")]
        public string InitialLoginTime { get; set; }
        [DisplayName("Login Count")]
        public long LoginCount { get; set; }
        //public string CreatedDate { get; set; }

    }
}
