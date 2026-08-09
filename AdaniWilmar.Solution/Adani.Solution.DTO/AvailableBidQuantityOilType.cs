using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AvailableBidQuantityOilType
    {
        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public int TotalChances { get; set; }
        public int ChancesLeft { get; set; }
        public decimal VolumeCapacity { get; set; }
        public decimal AvailableQuantity { get; set; }
    }

    public class AvailableBidQuantityInputDto
    {
        public long Id { get; set; }
        public long BiddingWindowId { get; set; }
    }
}
