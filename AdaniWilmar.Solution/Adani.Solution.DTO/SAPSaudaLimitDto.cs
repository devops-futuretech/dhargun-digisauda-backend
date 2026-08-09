using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HANASaudaLimitDtoList
    {
        public List<HANASaudaLimitDto> SaudaLimit { get; set; }

        public HANASaudaLimitDtoList()
        {
            SaudaLimit = new List<HANASaudaLimitDto>();
        }
    }

    public class HANASaudaLimitDto
    {       
        public string CustomerNo { get; set; }
        public string Division { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Limit_Qty { get; set; }
        public string Base_Unit_of_Measure { get; set; }
        public string Target_Value { get; set; }
        public string Currency_Key { get; set; }
        public string End_Date { get; set; }
        public string Old_Qty { get; set; }
        public string Old_Value { get; set; }

    }

    public class HANASaudaLimitList
    {
        public List<HANASaudaLimitDto> ContractLimit_Details { get; set; }

        public HANASaudaLimitList()
        {
            ContractLimit_Details = new List<HANASaudaLimitDto>();
        }
    }

    public class SAPSaudaLimitDto
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string PartnerFunction { get; set; }
        public string VerticalCode { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialDescription { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public decimal CustomerTotalQuantity { get; set; }
        public string UOM { get; set; }
        public decimal PendCont { get; set; }
        public decimal PendDO { get; set; }
        public decimal PendOBD { get; set; }
    }
}
