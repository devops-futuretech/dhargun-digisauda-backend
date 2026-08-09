using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class AddDealerVisitDto
    {
        public List<AddPendingSaudaRemarksDto> AddPendingSaudaRemarksDto { get; set; }
        public List<AddMarketScenarioDto> AddMarketScenarioDto { get; set; }
        public List<BdoCompetitorAddDto> BdoCompetitorAddDto { get; set; }
        public long CreatedBy { get; set; }
        public AddDealerVisitDto()
        {
            AddPendingSaudaRemarksDto = new List<AddPendingSaudaRemarksDto>();
            AddMarketScenarioDto = new List<AddMarketScenarioDto>();
            BdoCompetitorAddDto = new List<BdoCompetitorAddDto>();
        }
    }
    public class ProspectiveDealerAddListDto
    {
        public ProspectiveDealerAddDto ProspectiveDealerAddDto { get; set; }
        
        public long CreatedBy { get; set; }
        public ProspectiveDealerAddListDto()
        {
            ProspectiveDealerAddDto = new ProspectiveDealerAddDto();
        }
    }

    public class BdoCompetitorAddListDto
    {
        public List<BdoCompetitorAddDto> BdoCompetitorAddDto { get; set; }
        public long CreatedBy { get; set; }
        public BdoCompetitorAddListDto()
        {
            BdoCompetitorAddDto = new List<BdoCompetitorAddDto>();
        }
    }
}
