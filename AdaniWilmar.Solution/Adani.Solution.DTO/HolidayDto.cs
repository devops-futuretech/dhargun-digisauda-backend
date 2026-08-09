using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HolidayDto
    {
        public HolidayDto()
        {
            HolidayDetails = new List<HolidayDetailListDto>();
        }

        public string MonthName { get; set; }
        public int HolidayCount { get; set; }       

        public List<HolidayDetailListDto> HolidayDetails { get; set; }

    }


    public class HolidayDetailListDto
    {        
        public DateTime HolidayDate { get; set; }
        public string Day { get; set; }
        public string Remarks { get; set; }
    }


}
