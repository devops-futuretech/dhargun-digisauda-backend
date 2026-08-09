using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class HANASaudaStatusDto
    {
        public List<SaudaStatusDto>  Header { get; set; }
        public  HANASaudaStatusDto()
        {
            Header = new List<SaudaStatusDto>();
        }
    }
    public class SaudaStatusDto
    {
        public int SaudaStatusId { get; set; }
        public string SaudaNumber { get; set; }
    }
}
