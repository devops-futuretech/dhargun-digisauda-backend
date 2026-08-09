using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class ContractOpenBalanceRequestDto
    {
        public long DealerId { get; set; }
        public long SalesOrgId { get; set; }
        public long DistChnlId { get; set; }
        public long DivisionId { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string SalesOrg { get; set; }
        public string DistChnl { get; set; }
        public string Division { get; set; }
    }

    public class ContractOpenBalanceRequestSoleToPartyDto
    {
        public long DealerId { get; set; }
        public long SalesOrgId { get; set; }
        public long DistChnlId { get; set; }
        public long DivisionId { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string SalesOrg { get; set; }
        public string DistChnl { get; set; }
        public string Division { get; set; }
    }

    public class ContractOBRInputDto
    {
        public string DealerIds { get; set; }
        public long SalesOrgId { get; set; }
        public long DistChnlId { get; set; }
        public long DivisionId { get; set; }
       
    }


}
