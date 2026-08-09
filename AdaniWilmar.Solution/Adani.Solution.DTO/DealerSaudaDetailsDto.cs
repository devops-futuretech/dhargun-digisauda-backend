using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DealerSaudaDetailsDto
    {
        public decimal TotalSaudaLimit { get; set; }
        public decimal AvailableSaudaLimit { get; set; }
        public decimal OutstandingSaudaLimit { get; set; }
        public long BrokerId { get; set; }
        public string Broker { get; set; }
        public List<IncoTermsDto> IncoTermList { get; set; }
        public List<DepotDto> PlantDepotList { get; set; }
        public List<DepotDto> PlantDepotListNew { get; set; }
        public List<DropDownDto> BrokerList { get; set; }
        public List<UserDivisionSaudaLimitDto> UserDivisionSaudaLimitList { get; set; }
        public int SaudaValidityPeriod { get; set; }
        public long HighestBookedPlantId { get; set; }
        public DealerSaudaDetailsDto()
        {
            IncoTermList = new List<IncoTermsDto>();
            PlantDepotList = new List<DepotDto>();
            PlantDepotListNew = new List<DepotDto>();
            BrokerList = new List<DropDownDto>();
            UserDivisionSaudaLimitList = new List<UserDivisionSaudaLimitDto>();
        }
    }
    public class UserDivisionSaudaLimitDto
    {
        public long DivisionId { get; set; }
        public decimal SaudaLimit { get; set; }
        public decimal SaudaOrderQty { get; set; }
        public decimal PendingContractQty { get; set; }
        public decimal AvailableSaudaLimit { get; set; }
    }
    public class DealerSaudaDataDto
    {
        public long SalesOrganizationId { get; set; }
        public long DistrinbutionChannelId { get; set; }
        public long DivisionId { get; set; }
        public long StateId { get; set; }
        public long PlantId { get; set; }
    }
}
