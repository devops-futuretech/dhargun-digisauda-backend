using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AvailableBidQuantityDto
    {
        public string DealerName { get; set; }
        public int TotalChances { get; set; }
        public int ChancesLeft { get; set; }
        public List<AvailableBidQuantityOilType> AvailableBidQuantityOilType { get; set; }
        public AvailableBidQuantityDto()
        {
            AvailableBidQuantityOilType = new List<AvailableBidQuantityOilType>();
        }
    }
}
