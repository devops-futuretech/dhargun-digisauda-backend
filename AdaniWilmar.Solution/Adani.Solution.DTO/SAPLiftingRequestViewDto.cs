using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SAPLiftingRequestViewDto
    {
        public string CustomerCode { get; set; }
        public string ContractType { get; set; }
        public string SalesOrganization { get; set; }
        public string OilType { get; set; }
        public string MaterialNumber { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string UOM { get; set; }
        public long LiftingRequestDetailsId { get; set; }
        public string DistrbutionChannel { get; set; }
        public string Division { get; set; }
        public string ShipToPartyCode { get; set; }
        public DateTime LiftingRequestDate { get; set; }
        public DateTime? ApproveDate { get; set; }
    }
    public class HANALiftingRequestInquiryNumberDtoList
    {
        public List<LiftingRequestInquiryNumberDto> LiftingRequestInquiryNumberList { get; set; }

        public HANALiftingRequestInquiryNumberDtoList()
        {
            LiftingRequestInquiryNumberList = new List<LiftingRequestInquiryNumberDto>();
        }
    }
    public class LiftingRequestInquiryNumberDto
    {
        public long LiftingRequestId { get; set; }
        public string EnquiryNumber { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
    public class HANALiftingRequestEnquiryNumber
    {
        public List<SalesOrderCreate> Header { get; set; }
        public  HANALiftingRequestEnquiryNumber()
        {
            Header = new List<SalesOrderCreate>();
        }
    }

    public class SalesOrderCreate
    {
        public string TaskIdentification { get; set; }
        public string SAPContractNo { get; set; }
        public string ImpigerRequestNo { get; set; }
        public string DocumentType { get; set; }
        public string SalesOrg { get; set; }
        public string SoldTo { get; set; }
        public string ShipTo { get; set; }
        public string ApproveDate { get; set; }
        public string CustomerText { get; set; }
        public string SalesOrderNo { get; set; }
        public List<ItemDataDTO> ItemData { get; set; }

        public SalesOrderCreate() {
        
            ItemData = new List<ItemDataDTO>();
       }

    }

    public class ItemDataDTO
    {
        public string Qty { get; set; }
        public string UOM { get; set; }
        public string Material { get; set; }
        public string Plant { get; set; }
        public string ItemNo { get; set; }

    }
}

