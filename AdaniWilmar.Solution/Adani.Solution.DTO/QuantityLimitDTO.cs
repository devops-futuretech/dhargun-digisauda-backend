using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Adani.Solution.DTO
{
    public class QuantityLimitDTO : CommonResultDto
    {
        public long LoginUserId {  get; set; }
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string DivisionCode { get; set; }
        public string OilTypeName { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal QuantityLimit { get; set; }

    }
}
