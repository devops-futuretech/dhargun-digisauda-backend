using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class TradeTicketListDto
    {
        public List<HANATradeTicketViewDto> TradeTicketList { get; set; }
        public TradeTicketListDto()
        {
            TradeTicketList = new List<HANATradeTicketViewDto>();
        }
    }

    public class SAPTradeTicketViewDto
    {
        public long Id { get; set; }
        public string ContractType { get; set; }
        public string BookingType { get; set; }
        public string MaterialType { get; set; }
        public string UnitOfMeasure { get; set; }
        public string PlantOrVendor { get; set; }
        public string TradeTicketNumber { get; set; }
        public long TradeTicketId { get; set; }
        public long UomId { get; set; }
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
        public DateTime ContractDate { get; set; }
        public long DepotId { get; set; }
        public int ContractTypeId { get; set; }
        public int MaterialTypeId { get; set; }
        public int BookingTypeId { get; set; }
        public decimal ContractQuantity { get; set; }
        public decimal OtherElement { get; set; }
        public string Vertical { get; set; }     
        [Column(TypeName = "datetime2")]
        public DateTime? ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ValidTo { get; set; }

        //public string MATERIAL_TYPE1 { get; set; }
        //public string MATERIAL_TYPE2 { get; set; }
        //public string MATERIAL_TYPE3 { get; set; }
        //public string MATERIAL_TYPE4 { get; set; }
        //public string MATERIAL_TYPE5 { get; set; }
        //public string MATERIAL_TYPE6 { get; set; }
        //public string MATERIAL_TYPE7 { get; set; }
        //public string MATERIAL_TYPE8 { get; set; }
        //public string MATERIAL_TYPE9 { get; set; }
        //public string MATERIAL_TYPE10 { get; set; }

        //public decimal PRICE1 { get; set; }
        //public decimal PRICE2 { get; set; }
        //public decimal PRICE3 { get; set; }
        //public decimal PRICE4 { get; set; }
        //public decimal PRICE5 { get; set; }
        //public decimal PRICE6 { get; set; }
        //public decimal PRICE7 { get; set; }
        //public decimal PRICE8 { get; set; }
        //public decimal PRICE9 { get; set; }
        //public decimal PRICE10 { get; set; }

        //public decimal PRCOST1 { get; set; }
        //public decimal PRCOST2 { get; set; }
        //public decimal PRCOST3 { get; set; }
        //public decimal PRCOST4 { get; set; }
        //public decimal PRCOST5 { get; set; }
        //public decimal PRCOST6 { get; set; }
        //public decimal PRCOST7 { get; set; }
        //public decimal PRCOST8 { get; set; }
        //public decimal PRCOST9 { get; set; }
        //public decimal PRCOST10 { get; set; }

        //public decimal PROPORTION1 { get; set; }
        //public decimal PROPORTION2 { get; set; }
        //public decimal PROPORTION3 { get; set; }
        //public decimal PROPORTION4 { get; set; }
        //public decimal PROPORTION5 { get; set; }
        //public decimal PROPORTION6 { get; set; }
        //public decimal PROPORTION7 { get; set; }
        //public decimal PROPORTION8 { get; set; }
        //public decimal PROPORTION9 { get; set; }
        //public decimal PROPORTION10 { get; set; }
        public bool IsSAPDataSync { get; set; }
        public bool IsModified { get; set; }
        public List<SAPTradeTicketDetailsDto> TradeTicketDetail { get; set; }

        public SAPTradeTicketViewDto()
        {
            TradeTicketDetail = new List<SAPTradeTicketDetailsDto>();
        }
    }

    public class HANATradeTicketViewDto
    {        
        public string ContractType { get; set; }
        public string BookingType { get; set; }
        public string MaterialType { get; set; }
        public string UnitOfMeasure { get; set; }
        public string PlantOrVendor { get; set; }

        public string TradeTicketNumber { get; set; }       
        public DateTime ContractDate { get; set; }       
       
        public decimal ContractQuantity { get; set; }
        public decimal OtherElement { get; set; }
        public string Vertical { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ValidTo { get; set; }

        public decimal TotalCost { get; set; }
        public decimal TotalOilCost { get; set; }
        public decimal TotalProcessCost { get; set; }
        public string TTStatus { get; set; }
       

        public List<HANATradeTicketDetailsDto> TradeTicketDetail { get; set; }

        public HANATradeTicketViewDto()
        {
            TradeTicketDetail = new List<HANATradeTicketDetailsDto>();
        }
    }

    public class ErrorHANATradeTicketViewDto
    {
        public string ContractType { get; set; }
        public string BookingType { get; set; }
        public string MaterialType { get; set; }
        public string UnitOfMeasure { get; set; }
        public string PlantOrVendor { get; set; }

        public string TradeTicketNumber { get; set; }
        public DateTime ContractDate { get; set; }

        public decimal ContractQuantity { get; set; }
        public decimal OtherElement { get; set; }
        public string Vertical { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ValidFrom { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ValidTo { get; set; }

        public decimal TotalOilCost { get; set; }
        public decimal TotalProcessCost { get; set; }
        public decimal TotalCost { get; set; }
        public string TTStatus { get; set; }
    }
}
    

