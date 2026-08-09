using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaReleaseDto
    {
        public string SaudaNumber { get; set; }
        public string SaudaStatus { get; set; }
    }

    public class SaudaReleaseSAPToAPPDto
    {
        public string SaudaNumber { get; set; }
        public long SaudaStatusId { get; set; }
        public string Remarks { get; set; }
    }

    public class SaudaConversionSAPToAPPDto
    {
        public string SaudaNumber { get; set; }
        public long SaudaStatusId { get; set; }
        public string Remarks { get; set; }
    }
}
