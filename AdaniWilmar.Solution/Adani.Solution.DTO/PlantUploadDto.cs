using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PlantUploadDto : CommonResultDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Zone { get; set; }
        public string StateName { get; set; }
        public string TerritoryName { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string Pincode { get; set; }
        public string Address { get; set; }
        public string IsActive { get; set; }
        public long CreatedBy { get; set; }
        public int StorageTypeId { get; set; }
    }

    public class RakeUploadDto : CommonResultDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Zone { get; set; }
        public string StateName { get; set; }
        public string TerritoryName { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string Pincode { get; set; }
        public string Address { get; set; }
        public string IsActive { get; set; }
        public string DepotCode { get; set; }
        public long CreatedBy { get; set; }
        public int StorageTypeId { get; set; }
        public string MappedStateName { get; set; }
        public string MappedPlantCode { get; set; }
    }
}
