using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HANAChequeStatusDtoList
    {
        public List<SAPChequeStatusDto> SAPChequeStatusDtos { get; set; }
        public  HANAChequeStatusDtoList()
        {
            SAPChequeStatusDtos = new List<SAPChequeStatusDto>();
        }
    }

   public class SAPChequeStatusDto
    { 
        public string ControllingArea { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string ChequeNo { get; set; }
        public string NameOfBank { get; set; }
        public string BranchName { get; set; }        
    }
}
