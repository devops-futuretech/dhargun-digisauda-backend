using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class FillerSkuInputDto
    {
        public long LoginUserId { get; set; }
        public long DealerId { get; set; }
        public decimal InputCases { get; set; }
        public decimal Volumepercentage { get; set; }
        public decimal Weightpercentage { get; set; }
        public bool IsMultipleSku { get; set; }
        public decimal VehicleSize { get; set; }
        public long PlantId { get; set; }
    }
}
