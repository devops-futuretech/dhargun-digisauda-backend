using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Adani.Solution.DTO
{
    public class CreditLimitDto
    {
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CreditExposure { get; set; }
    }
    public class CreditLimitExportDto
    {
        [DisplayName("Customer Code")]
        public string CustomerCode { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("Credit Limit(In Lakhs)")]
        public decimal CreditLimt { get; set; }
        [DisplayName("Credit Exposure(In Lakhs)")]
        public decimal CreditExposure { get; set; }
        [DisplayName("Gross Exposure(In Lakhs)")]
        public decimal GrossExposure { get; set; }
        [DisplayName("Open Exposure(In Lakhs)")]
        public decimal OpenExposure { get; set; }
        [DisplayName("Total Receivable(In Lakhs)")]
        public decimal TotalReceivable { get; set; }
        //[DisplayName("Available Limit(In Lakhs)")]
        //public decimal AvailableLimit { get; set; }
        [DisplayName("Over Due")]
        public decimal OverDue { get; set; }
        [DisplayName("Tomorrow's Due")]
        public decimal TommorrowDue { get; set; }
    }
    public class CreditLimitInputDto
    {
        public long LoginUserId { get; set; }
        public long PackGroupId { get; set; }
        public List<long> NationalHeadIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

}
