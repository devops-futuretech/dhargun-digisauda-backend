using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HANASAPCustomerDtoList
    {
        public List<HANASAPCustomerDto> SAPUserList { get; set; }

        public HANASAPCustomerDtoList()
        {
            SAPUserList = new List<HANASAPCustomerDto>();
        }
    }
    public class HANASAPCustomerDto
    {
        public string Code { get; set; }
        public string UserCode { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
        public string ADRNR { get; set; }
        public string GSTN { get; set; }
        public string District { get; set; }
        public string DeliveringPlant { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string CentralDeletionFlag { get; set; }
        public string VerticalCode { get; set; }
        public string FSSAINumber { get; set; }
        public string AccountGroup { get; set; }
        public int RoleId { get; set; }       
        public string CustomerGroup { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
    }

    public class SAPCustomerDto
    {
        public string Code { get; set; }
        public string UserCode { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
        public string ADRNR { get; set; }
        public string GSTN { get; set; }
        public string District { get; set; }
        public string DeliveringPlant { get; set; }
        public string MobileNumber { get; set; }

        public string Email { get; set; }
        public string State { get; set; }
        public string CentralDeletionFlag { get; set; }
        public string VerticalCode { get; set; }
        public string FSSAINumber { get; set; }
        public string AccountGroup { get; set; }

        public string ErrorMessage { get; set; }
        
        public int RoleId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public string SalesOrganization { get; set; }
        public string CustomerGroup { get; set; }
       
       
        
    }
}
