using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaTypeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class BookingTypeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class MaterialTypesDto 
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
