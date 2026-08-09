using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ResponsesDto
    {
        public List<Dictionary<string, string>> LabelListDto;
        public List<GamificationDashboardDto> GamificationDashboardDto;
    }
    public class LabelListDto
    {
        public Dictionary<string, string> Label { get; set; }

    }
}
