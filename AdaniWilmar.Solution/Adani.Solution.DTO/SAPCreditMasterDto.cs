using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{

    public class HANACreditMasterDtoList
    {
        public List<HANACreditMasterDto> CreditMasterList { get; set; }

        public HANACreditMasterDtoList()
        {
            CreditMasterList = new List<HANACreditMasterDto>();
        }
    }

    public class HANACreditMasterDto
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CCreditArea { get; set; }
        public string CreditAccountNumber { get; set; }
        public string RiskCat { get; set; }
        public string Curr { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CreditExposure { get; set; }
        public decimal GrossExposure { get; set; }
        public decimal OpenExposure { get; set; }
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

    }

    public class SAPCreditMasterDto
    {
        public string CustomerAC  { get; set; }
        public string CreditControl { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal TotalReceivable { get; set; }
        public decimal SpecialLiabil { get; set; }        
        public decimal CreditExposure { get; set; }
        public decimal CreditLimitPercentage { get; set; }
        public string CustomerCode { get; set; }
        public string CCreditArea { get; set; }
        public string CreditAccountNumber { get; set; }
        public string RiskCat { get; set; }
        public string Curr { get; set; }
        public decimal SalesValue { get; set; }       
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

    }

    public class HANAOpenBalAndOpenContractDTOList
    {
        public OpenBalAndOpenContractDTO Records { get; set; }
        public HANAOpenBalAndOpenContractDTOList()
        {
            Records = new OpenBalAndOpenContractDTO();
        }
    }

    public class OpenBalAndOpenContractDTO
    {
        public string SalesOrg { get; set; }
        public string DistChannel { get; set; }
        public string Division { get; set; }
        public List<OpenBal> OpenBal { get; set; }
        public List<OpenContract> OpenContract { get; set; }

        public OpenBalAndOpenContractDTO()
        {
            OpenBal = new List<OpenBal>();
            OpenContract = new List<OpenContract>();
        }
    }

    public class OpenBal
    {
        public string CreditLimit { get; set; }
        public string OpenOrders { get; set; }
        public string DeliveryValue { get; set; }
        public string BillingDocumentValue { get; set; }
        public string TotalExposure { get; set; }
        public string SoldToParty { get; set; }
    }

    public class OpenContract
    {
        public string SaudaNumber { get; set; }
        public string ValidTo { get; set; }
        public string Contract_CreatedDate { get; set; }        
        public string Material { get; set; }
        public string Price { get; set; }
        public string OpenQTY { get; set; }
        public string SoldToParty { get; set; }
        public string OpenSOQTY { get; set; }
    }

}
