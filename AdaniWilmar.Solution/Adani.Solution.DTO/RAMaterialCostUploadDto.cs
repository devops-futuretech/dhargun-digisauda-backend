using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class RAMaterialCostUploadDto : CommonResultDto
    {
        public string PlantCode { get; set; }
        public string VerticalCode { get; set; }
        //OilWise
        public string OilType { get; set; }
        public decimal RateOrMT { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        //public string IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
}
