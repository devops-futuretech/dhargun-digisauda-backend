using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PendingContractReportDto : LoginUserIdDto, IAPIInputDTO
    {
        //public long VerticalId { get; set; }
        public long RoleId { get; set; }

        public List<long> BDOIds { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> ZoneIds { get; set; }
        public List<long> StateIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class PendingContractReportOutputDto
    {
        public string PlantName { get; set; }
        public string PlantCode { get; set; }
        public string State { get; set; }
        public long StateId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string OilType { get; set; }

        public decimal PendingQtyCases { get; set; }
        public decimal PendingQty_MT { get; set; }
        public decimal BasicRatePerCase { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidQuantityCase { get; set; }
        public decimal LiftingQtyCase { get; set; }
        public decimal qtyInCase { get; set; }
        public decimal ActualBilledQty { get; set; }
        public decimal LiftingQty { get; set; }

        public string IncoTerms { get; set; }
        public string ContractNo { get; set; }
        public string SAPContractNo { get; set; }
        public DateTime SaudaDate { get; set; }
        public DateTime ContractValidFrom { get; set; }
        public DateTime ContractValidTo { get; set; }
        public string BrokerName { get; set; }

    }

    public class PendingContractStatistics
    {
        public DateTime ContractValidTo { get; set; }
        public decimal PendingQuantityInMT { get; set; }
    }

}
