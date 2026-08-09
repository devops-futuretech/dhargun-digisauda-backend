using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class UserDiscount: CommonResultDto
    {
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        public string MaterialCode { get; set; }
        public string EmployeeCode { get; set; }
        public string DiscountReason { get; set; }
        public string StateName { get; set; }
        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
    }

    public class GeographyDiscount : CommonResultDto
    {
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        public string OilType { get; set; }
        public string PackGroup { get; set; }
        public string MaterialCode { get; set; }
        public string DiscountReason { get; set; }
        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public string Zone { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string IsActive { get; set; }
        public string PackType { get; set; }
    }

    public class GeographyDiscountImportStatus 
    {
        public long Id { get; set; }
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        public string MaterialCode { get; set; }
        public string DiscountReason { get; set; }
        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public string Zone { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Message { get; set; }
        public string OilType { get; set; }
        public string PackGroup { get; set; }
        public string PackType { get; set; }
        public bool IsActive { get; set; }
    }
}
