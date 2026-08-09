using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UpdateStateDto
    {
        public int StateId { get; set; }
        public string EncryptedId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public long ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
