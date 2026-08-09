using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class Holiday : Auditable
    {
        public string HolidayName { get; set; }

        public DateTime HolidayDate { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }
    }
}
