using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LineUploadDto : CommonResultDto
    {
        public string LineName { get; set; }
        public string IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string DistributorCode { get; set; }
        public string MaterialCode { get; set; }
    }
}
