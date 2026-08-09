using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.DTO
{
    public class TruckReportInputDto : VerticalIdDto
    {
        public long LoginUserId { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        public int PageNo { get; set; }
        public List<long> DealerIds { get; set; }
        public List<long> BdoIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> NationalHeadIds { get; set; }
    }
}
