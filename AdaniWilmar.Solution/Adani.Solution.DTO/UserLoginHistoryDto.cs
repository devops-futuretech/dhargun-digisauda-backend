using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserLoginHistoryDto
    {

        public long LoginUserId { get; set; }

        public DateTime LoginDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

    }
}
