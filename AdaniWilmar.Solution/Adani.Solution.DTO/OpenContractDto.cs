using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class OpenContractDto
    {
        public long UserId { get; set; }
        public long SaudaOrderId { get; set; }
        public string SaudaNumber { get; set; }
        public string MaterialCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ContractValidTo { get; set; }
        public string ContractValidFrom { get; set; }
        public decimal BasicRate { get; set; }       
        public decimal PendingQuantityInCase { get; set; }       
        public decimal SaudaQuantity { get; set; }
        public long SalesOrgId { get; set; }
        public long DistChnlId { get; set; }
        public long DivisionId { get; set; }
        public decimal TotalValue { get; set; }
        public bool IsSaudaExtended { get; set; }
        public long CreatedBy { get; set; }
        //[Column(TypeName = "datetime2")]
        //public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        //[Column(TypeName = "datetime2")]
        //public DateTime? ModifiedDate { get; set; }
    }
}
