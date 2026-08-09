using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UpdateDistrictDto
    {
        public string EncryptedId { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int StateId { get; set; }
        public int TerritoryId { get; set; }
        public bool IsActive { get; set; }
        public int ModifiedBy { get; set; }
    }
}
