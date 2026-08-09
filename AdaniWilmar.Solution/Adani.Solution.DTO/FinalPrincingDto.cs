using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FinalPrincingDto : IAPIInputDTO
    {
        public int SaudaBookingTypeId { get; set; }
        public int VerticalId { get; set; }
        public int OilTypeId { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public int OilPackingTypeId { get; set; }
        public int TransportModeId { get; set; }
        public string CounterBid { get; set; }
        public string BPCPJumb { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class FinalPriceCountDto
    {
        public int StateId { get; set; }
        public int VerticalId { get; set; }
        public string VerticalName { get; set; }
        public string StateName { get; set; }
        public int RecordCount { get; set; }
        public int SaudaBookingTypeId { get; set; }
        public string SaudaBookingType { get; set; }
    }
}
