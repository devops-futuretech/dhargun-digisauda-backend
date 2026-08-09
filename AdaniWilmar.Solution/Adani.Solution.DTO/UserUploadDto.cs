using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class UserUploadDto : CommonResultDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public string CompanyCode { get; set; }
        public string Email { get; set; }
        public string ZoneName { get; set; }
        public string StateName { get; set; }
        public string TerritoryName { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string Pincode { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Designation { get; set; }
        public string IsActive { get; set; }
        public string RoleName { get; set; }
        public string CustomerCode { get; set; }
        public long CreatedBy { get; set; }
        public string Headquarters { get; set; }
        //public string OrgReportingToUserCode { get; set; }
        //public string SalesReportingToUserCode { get; set; }
        public string ReportingToUserCode { get; set; }
        public string SaudaBookingType { get; set; }
        public string Password { get; set; }
        public string EncryptedPassword { get; set; }
        //public string CustomerGroupOneName { get; set; }
        //public string CustomerGroupTwoName { get; set; }
        public string CMSReportingToUserCode{ get; set; }
    }
}
