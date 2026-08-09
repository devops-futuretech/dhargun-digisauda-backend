using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class GeographyUploadDto : CommonResultDto
    {
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string TerritoryName { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public long CreatedBy { get; set; }
        public string IsActive { get; set; }
    }
}
