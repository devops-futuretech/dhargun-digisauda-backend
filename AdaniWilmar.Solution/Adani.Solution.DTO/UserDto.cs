using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class UserDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CustomerGroup { get; set; }        
        public long? VerticalId { get; set; }
        public long? SaudaBookingTypeId { get; set; }

    }
    public class UserRoleIdDto
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string Username { get; set; }
        public string Code { get; set; }
    }
}
