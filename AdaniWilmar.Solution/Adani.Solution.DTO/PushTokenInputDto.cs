using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PushTokenInputDto : LoginUserIdDto
    {
        public string PushToken { get; set; }
        public int RegistrationTypeId { get; set; }
    }
}
