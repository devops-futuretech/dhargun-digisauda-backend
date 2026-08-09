using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class RetailerUploadDto : CommonResultDto
    {
        public string AccountName { get; set; }
        public string Code { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string SPFZoneName { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string TerritoryName { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string IsActive { get; set; }
        public string FreightZoneName { get; set; }
        public string FreightRouteName { get; set; }  
        public string DistributorName { get; set; }
        public string AccountManager { get; set; }
        public string AccountType { get; set; }
        public string AreaName { get; set; }
        public string OwnersName { get; set; }
        public string DecisionMakerName { get; set; }
        public string DecisionMakerNumber { get; set; }
        public string ChefName { get; set; }
        public string ChefNumber { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string VerticalCode { get; set; }
        public string SalesOrganizationName { get; set; }
        public string DistributionChannelName { get; set; }
        public string DealerCode { get; set; }
        public long CreatedBy { get; set; }
    }    
}
