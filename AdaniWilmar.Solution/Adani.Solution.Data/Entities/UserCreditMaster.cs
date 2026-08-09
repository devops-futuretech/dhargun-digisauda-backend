using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserCreditMaster 
    {
        [Required] 
        public long UserId { get; set; }
        public long SalesOrgId { get; set; }
        public long DistChnlId { get; set; }
        public long DivisionId { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CreditExposure { get; set; }
        public decimal BillingDocumentValue { get; set; }
        public decimal DeliveryValue { get; set; }
        public decimal OpenOrders { get; set; }
        public bool Isactive { get; set; }
        public bool IsSAPData { get; set; }
        public long Id { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedDate { get; set; }
        //public string CCreditArea { get; set; }
        public string CreditAccountNumber { get; set; }
        public string RiskCat { get; set; }
        public string Curr { get; set; }
        public decimal SalesValue { get; set; }
        public decimal TotalReceivable { get; set; }
        public decimal SaudaDepC { get; set; }
        public decimal SecDepH { get; set; }
        public decimal BankGuarM { get; set; }
        public decimal AdvanceA { get; set; }
        public decimal DueToday { get; set; }
        public decimal TomorrowsDue { get; set; }
        public decimal Overdue { get; set; }
        public decimal NotDue { get; set; }
        public string NextIntRev { get; set; }
        public string Blocked { get; set; }
        public decimal TotalLimit { get; set; }
        public decimal IndividLimit { get; set; }
        public decimal AvailableCreditLimit { get; set; }       
        public virtual User User { get; set; }
    }
}
