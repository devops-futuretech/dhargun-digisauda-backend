using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SAPSaudaExtensionDto
    {
        public long Id { get; set; }
        public string SaudaNumber { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public string SAPRemarks { get; set; }
    }

    public class SAPSaudaExtensionOutputDto
    {
        public int ListCount { get; set; }

        public List<SAPSaudaExtensionDto> SAPSaudaExtensionList { get; set; }

    }


    public class SAPSaudaInputDto
    {
        public string SaudaNumbers { get; set; }
        public long StatusId { get; set; }
        public long LoginUserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int PageNo { get; set; }
    }
}
