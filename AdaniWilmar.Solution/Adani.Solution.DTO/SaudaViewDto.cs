using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HANASaudaListDto
    {
        public List<HANADirectSaudaDto> SaudaViewList { get; set; }
        public HANASaudaListDto()
        {
            SaudaViewList = new List<HANADirectSaudaDto>();
        }
    }

    public class HANASaudaViewList 
    {
        public List<SaudaCreateDto> Header { get; set; }
        public  HANASaudaViewList()
        {
            Header = new List<SaudaCreateDto>();
        }
    }

    public class HANASaudaModificationViewList
    {
        public List<SaudaModificationCreateDto> Header { get; set; }
        public HANASaudaModificationViewList()
        {
            Header = new List<SaudaModificationCreateDto>();
        }
    }

    public class SaudaViewDto : EntityDto
    {
        public string SaudaNumber { get; set; }
        public string SOType { get; set; }
        public string VerticalName { get; set; }
        public string CustomerPoNumber { get; set; }
        public string ContractTypeName { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string SoldToParty { get; set; }
        public string ShipToParty { get; set; }
        public string Sku { get; set; }
        public decimal BidQuantity { get; set; }
        public DateTime DocumentDate { get; set; }
        public string CustomerGroup { get; set; }
        public string PriceGroup { get; set; }
        public string Usage { get; set; }
        public string INCO1 { get; set; }
        public string INCO2 { get; set; }
        public DateTime BillDate { get; set; }
        public string DeliveryPriority { get; set; }
        public string UserDepotMapping { get; set; }
        public string PickingPoint { get; set; }
        public int MaximumNumberDeliveries { get; set; }
        public string TradeTicketNumber { get; set; }
        public string ConditionType1 { get; set; }
        public decimal BidAmount { get; set; }
        public string ConditionType2 { get; set; }
        public decimal Rate2 { get; set; }
        public string CustomerPOType { get; set; }
        public string Uom { get; set; }
        public string ConditionType3 { get; set; }
        public decimal Rate3 { get; set; }
        public string ConditionType4 { get; set; }
        public decimal Rate4 { get; set; }
        public string PriceListType { get; set; }

        public long VerticalId { get; set; }
        public long BookingId { get; set; }
        public DateTime CustomerPoDate { get; set; }
        public string CustomerCode { get; set; }
        public bool IsSAPDataSync { get; set; }
        public bool IsModified { get; set; }
        public string BillToParty { get; set; }
        public string Payer { get; set; }
        public string Broker { get; set; }
       
        public string Division { get; set; }
        


        //new fields
        public string TaskIdentification { get; set; }
        public string DocumentType { get; set; }
        public string SalesOrg { get; set; }
        public string DistCh { get; set; }
        //public DateTime ValidFrom { get; set; }
        //public DateTime ValidTo { get; set; }
        public string SoldTo { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        //public string Payer { get; set; }
        //public string Broker { get; set; }
        //public string INCO1 { get; set; }
        //public string INCO2 { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public long ImpigerRequestNo { get; set; } //SaudaID
        public string SAPContractNo { get; set; }
        public List<SAPDataItemData> ItemData { get; set; }

        public SaudaViewDto()
        {
            ItemData = new List<SAPDataItemData>();
        }
    }

    public class SaudaCreateDto 
    {
        public string TaskIdentification { get; set; }
        public string DocumentType { get; set; }
        public string Division { get; set; }
        public string SalesOrg { get; set; }
        public string DistCh { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string SoldTo { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public string Payer { get; set; }
        public string Broker { get; set; }
        public string INCO1 { get; set; }
        public string INCO2 { get; set; }
        public string PONumber { get; set; }
        public string PODate { get; set; }
        public string ImpigerRequestNo { get; set; } //SaudaID
        public string SAPContractNo { get; set; }
        public List<SAPDataItemData> ItemData { get; set; }       
       

        public SaudaCreateDto()
        {
            ItemData = new List<SAPDataItemData>();
        }
    }

    public class SaudaCreateSAPToAPPDto
    {
        public string TaskIdentification { get; set; }
        public string DocumentType { get; set; }
        public string Division { get; set; }
        public string SalesOrg { get; set; }
        public string DistCh { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string SoldTo { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public string Payer { get; set; }
        public string Broker { get; set; }
        public string INCO1 { get; set; }
        public string INCO2 { get; set; }
        public string PONumber { get; set; }
        public string PODate { get; set; }
        public string ImpigerRequestNo { get; set; } //SaudaID
        public string SAPContractNo { get; set; }
        public List<SAPDataItemDataSapToApp> ItemData { get; set; }

        public SaudaCreateSAPToAPPDto()
        {
            ItemData = new List<SAPDataItemDataSapToApp>();
        }
    }

    public class SAPDataItemData
    {
        public string Qty { get; set; }
        public string UOM { get; set; }
        public string Material { get; set; }
        public string StorageLocation { get; set; }
        public string Plant { get; set; }
        public string ConditionType { get; set; }
        public string Amount { get; set; }

    }

    public class SAPDataModificationItemData
    {
        public string ItemNumber { get; set; }
        public string Qty { get; set; }
        public string UOM { get; set; }
        public string Material { get; set; }
        public string StorageLocation { get; set; }
        public string Plant { get; set; }
        public string ConditionType { get; set; }
        public string Amount { get; set; }
    }

    public class SaudaModificationCreateDto
    {
        public string TaskIdentification { get; set; }
        public string DocumentType { get; set; }
        public string Division { get; set; }
        public string SalesOrg { get; set; }
        public string DistCh { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string SoldTo { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public string Payer { get; set; }
        public string Broker { get; set; }
        public string INCO1 { get; set; }
        public string INCO2 { get; set; }
        public string PONumber { get; set; }
        public string PODate { get; set; }
        public string ImpigerRequestNo { get; set; } //SaudaModificationID
        public string SAPContractNo { get; set; }
        public List<SAPDataModificationItemData> ItemData { get; set; }

        public SaudaModificationCreateDto()
        {
            ItemData = new List<SAPDataModificationItemData>();
        }
    }

    public class SAPDataItemDataSapToApp
    {
        public string Qty { get; set; }
        public string UOM { get; set; }
        public string Material { get; set; }
        public string StorageLocation { get; set; }
        public string Plant { get; set; }
        public string ConditionType { get; set; }
        public string Amount { get; set; }
        public string ItemNo { get; set; }
        public string PR10GST { get; set; }
        public string PR10Amount { get; set; }

    }


    public class HANADirectSaudaDto 
    {
        public string SaudaNumber { get; set; }
        public string SOType { get; set; }
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        //public string VerticalName { get; set; }
        public string CustomerPoNumber { get; set; }
        public string ContractTypeName { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string SoldToParty { get; set; }
        public string ShipToParty { get; set; }
        public string Sku { get; set; }
        public decimal BidQuantity { get; set; }
        public DateTime DocumentDate { get; set; }
        public string CustomerGroup { get; set; }       
        public string PriceGroup { get; set; }
        public string Usage { get; set; }
        public string INCO1 { get; set; }
        public string INCO2 { get; set; }
        public DateTime BillDate { get; set; }
        public string DeliveryPriority { get; set; }
        public string UserDepotMapping { get; set; }
        public string PickingPoint { get; set; }
        public int MaximumNumberDeliveries { get; set; }
        public string TradeTicketNumber { get; set; }
        public string ConditionType1 { get; set; }
        public decimal BidAmount { get; set; }
        public string ConditionType2 { get; set; }
        public decimal Rate2 { get; set; }
        public string CustomerPOType { get; set; }
        public string Uom { get; set; }
        public string ConditionType3 { get; set; }
        public decimal Rate3 { get; set; }
        public string ConditionType4 { get; set; }
        public decimal Rate4 { get; set; }
        public string BillToParty { get; set; }
        public string Payer { get; set; }
        public string Broker { get; set; }        
    }

    public class OpenContractRequestDTO
    {
        public string SoldToParty { get; set; }       
    }
    public class OpenContractRequestInputDto
    {
        public OpenContractRequestDTOList OpenContractBalReq { get; set; }

        public OpenContractRequestInputDto()
        {
            OpenContractBalReq = new OpenContractRequestDTOList();
        }
    }
    public class OpenContractRequestDTOList
    {       
        public string SalesOrg { get; set; }
        public string DistChannel { get; set; }
        public string Division { get; set; }
        public List<OpenContractRequestDTO> Records { get; set; }

        public OpenContractRequestDTOList()
        {
            Records = new List<OpenContractRequestDTO>();
        }
    }

    public class DarwinboxAPIRequestDTO
    {
        public string api_key { get; set; }
        public string datasetKey { get; set; }
        public string last_modified { get; set; }
    }

    public class DarwinboxEmployeeListDTO
    {
        public string employee_id { get; set; }
        public string full_name { get; set; }       
        public string department_name { get; set; }
        public string direct_manager_name { get; set; }       
        public string office_area { get; set; }
    }

    public class DarwinboxAPIResponsetDTO
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<DarwinboxEmployeeListDTO> employee_data { get; set; }

        public DarwinboxAPIResponsetDTO()
        {
            employee_data = new List<DarwinboxEmployeeListDTO>();
        }
    }

  


}
