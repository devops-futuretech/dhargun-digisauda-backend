using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class DayOfWeekName : Entity
    {
        public string Name { get; set; }
        public bool IsHoliday { get; set; }
        public int SortOrder { get; set; }
    }
}
