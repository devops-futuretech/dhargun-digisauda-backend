using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PlantDepotMappingUploadDto : CommonResultDto
    {
        public string PlantCode { get; set; }
        public string DepotCode { get; set; }
    }
}
