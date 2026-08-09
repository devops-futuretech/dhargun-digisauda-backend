using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AWLCustomerLedgerDto
    {
        public HANACustomerLedgerDtoList CustomerLedger_Response { get; set; }       
    }
    public class HANACustomerLedgerDtoList
    {
        public List<HANACustomerLedgerDto> Records { get; set; }
        public HANACustomerLedgerDtoList()
        {
            Records = new List<HANACustomerLedgerDto>();
        }
    }
   public class HANACustomerLedgerDto
   {
        public string Ref_Number { get; set; }
        public string Document_Date { get; set; }
        public string Due_Date { get; set; }
        public string Document_Type { get; set; }
        public string Amount { get; set; }
        public string ShipTo { get; set; }
        public string Currency { get; set; }
        public string Company_Code { get; set; }
        public string Customer_Code { get; set; }       
    }

    public class SAPCustomerLedgerDto
    {
        public string UserCode { get; set; }
        public string PdfUrl { get; set; }
        public string PdfFileName { get; set; }
    }

    public class SAPCustomerLedgerRequestDTO
    {
        public string Customer_Number { get; set; }
        public string Company_Code { get; set; }        
    }

    public class SAPCustomerLedgerRequestListDTO
    {
        public List<SAPCustomerLedgerRequestDTO> Records;
        public SAPCustomerLedgerRequestListDTO()
        {
            Records = new List<SAPCustomerLedgerRequestDTO>();
        }
    }
}
