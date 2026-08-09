using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PercentileNumbersUploadDto : CommonResultDto
    {
        public long OilTypeId { get; set; }
        public long PackGroupId { get; set; }
        public long PercentileNumbers { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string IsActive { get; set; }
    }
}
