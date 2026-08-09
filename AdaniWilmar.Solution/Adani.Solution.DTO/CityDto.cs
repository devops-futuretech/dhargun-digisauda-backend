using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CityDto
    {
        public int CityId { get; set; }
        public string EncryptedId { get; set; }
        public string CityName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public bool IsActive { get; set; }
        public int StateId { get; set; }
        public int TerritoryId { get; set; }
        public string TerritoryName { get; set; }
        public string StateName { get; set; }
        public int LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
