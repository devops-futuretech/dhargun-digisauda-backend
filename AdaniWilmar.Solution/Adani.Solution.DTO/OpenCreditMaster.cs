using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class OpenCreditMaster
    {
        public long UserId { get; set; }
        public long SalesOrgId { get; set; }
        public long DistChnlId { get; set; }
        public long DivisionId { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CreditExposure { get; set; }
        public decimal OpenOrders { get; set; }
        public decimal DeliveryValue { get; set; }
        public decimal BillingDocumentValue { get; set; }
        public decimal AvailableCreditLimit { get; set; }
        public long CreatedBy { get; set; }
        //[Column(TypeName = "datetime2")]
        //public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        //[Column(TypeName = "datetime2")]
        //public DateTime? ModifiedDate { get; set; }
    }
        
}
