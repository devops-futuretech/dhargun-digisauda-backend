using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DropDownInputDto
    {
        public List<long> VerticalIds { get; set; }
        public List<long> UserIds { get; set; }
        public List<long> CustomerGroupIds { get; set; }

        public long VerticalId { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> PackGroupIds { get; set; }
        public List<long> SubCategoryIds { get; set; }
        public List<long> CityIds { get; set; }
        public long PackTypeId { get; set; }
    }
}
