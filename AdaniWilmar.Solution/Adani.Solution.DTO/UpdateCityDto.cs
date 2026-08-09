using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UpdateCityDto
    {
        public int CityId { get; set; }
        public string EncryptedId{ get; set; }
        public string CityName { get; set; }
        public int DistrictId { get; set; }
        public int TerritoryId { get; set; }
        public bool IsActive { get; set; }
        public int ModifiedBy { get; set; }
    }
}
