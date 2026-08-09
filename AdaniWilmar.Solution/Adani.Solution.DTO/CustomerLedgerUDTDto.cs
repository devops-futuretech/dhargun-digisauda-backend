using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class CustomerLedgerUDTDto
    {
        public string Reference { get; set; }
        public string PostingDate { get; set; }
        public string DueDate { get; set; }
        public string DocumentType { get; set; }
        public decimal Balance { get; set; }
        public long UserId { get; set; }       
        public string UserCode { get; set; }
        public string CompanyCode { get; set; }
        public string Currency { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public long CreatedBy { get; set; }
        //[Column(TypeName = "datetime2")]
        //public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        //[Column(TypeName = "datetime2")]
        //public DateTime? ModifiedDate { get; set; }
    }
}
