using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaLimitDto
    {
        public long CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string Employee { get; set; }
        public decimal SaudaLimit { get; set; }
        //public decimal PendingQuantity { get; set; }
        public decimal AvailableSaudaLimit { get; set; }
        public string State { get; set; }
        //public decimal PendingContract { get; set; }
        //public decimal PendingDO { get; set; }
        //public decimal PendingOBD { get; set; }
        //public decimal PendingQuantityInPortal { get; set; }

        public string SaudaNumber { get; set; }
        public decimal SaudaOrderQty { get; set; }
        public decimal SaudaOrderQtyCase { get; set; }
        public decimal PendingContractQty { get; set; }
        public decimal PendingContractQtyCase { get; set; }
        public decimal AvailableSaudaLimitCase { get; set; }

        public long DivisionId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public string DivisionName { get; set; }
        public string SalesOrganizationName { get; set; }
        public string DistributionChannelName { get; set; }
    }

    public class SaudaLimitExportDto
    {
        [DisplayName("Customer Code")]
        public string CustomerCode { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("State")]
        public string State { get; set; }
        [DisplayName("Employee")]
        public string Empoloyee { get; set; }
        [DisplayName("Sauda Order Quantity")]
        public decimal SaudaOrderQuantity { get; set; }
        [DisplayName("Pending Contract Quantity")]
        public decimal PendingContratQuantity { get; set; }
        [DisplayName("Contract Limit(MT)")]
        public decimal ContractLimt { get; set; }
        [DisplayName("Sauda Order Quantity In MT")]
        public decimal SaudaOrderQuantityInMt { get; set; }
        [DisplayName("Pending Contract Quantity In MT")]
        public decimal PendingContractInMt { get; set; }
        [DisplayName("Available Contract Limit(MT)")]
        public decimal AvailableContractInMt { get; set; }
        [DisplayName("Sales Organization")]
        public string SalesOrganization { get; set; }
        [DisplayName("Distribution Channel")]
        public string DistributionChannel { get; set; }
        [DisplayName("Division")]
        public string Division { get; set; }
    }
}
