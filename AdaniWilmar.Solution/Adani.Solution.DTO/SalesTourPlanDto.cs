using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SalesTourPlanDto
    {
    }

    public class SalesTourPlanParamDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long BdoId { get; set; }
        public long DealerId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ProspectiveDealerVisitDto
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string Pincode { get; set; }
        public bool IsActive { get; set; }
        public decimal ProspectiveSales { get; set; }
        public decimal ProspectiveInterestLevel { get; set; }
        public decimal BusinessPotentialPeryear { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
    }

    public class PendingSaudaRemarksDto
    {
        public long SaudaId { get; set; }
        public string DealerName { get; set; }        
        public string Remarks { get; set; }
        public string Status { get; set; }        
    }

    public class MarketScenariosDto
    {        
        public string DealerName { get; set; }
        public string Title { get; set; }
        public string Remarks { get; set; }
    }

    public class BdoCompetitorsDto
    {
        public BdoCompetitorsDto()
        {
            BdoCompetitorSkuDetails = new List<BdoCompetitorSkusDto>();
        }
        public long Id { get; set; }
        public string CompetitorName { get; set; }
        public int StateName { get; set; }
        public int DistrictName { get; set; }
        public int CityName { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public string DealerName { get; set; }
        public string Attachments { get; set; }

        public List<BdoCompetitorSkusDto> BdoCompetitorSkuDetails { get; set; }
    }

    public class BdoCompetitorSkusDto
    {
        public long BdoCompetitorId { get; set; }
        public string BdoCompetitorName { get; set; }
        public string SkuName { get; set; }
        public decimal QuanityPerMt { get; set; }
        public decimal Price { get; set; }
    }

    public class AttachmentInputDto
    {
        public long RecordId { get; set; }
        public long PageId { get; set; }
    }
    public class AttachmentFileDto
    {
        public long RecordId { get; set; }
        public string FileUrl { get; set; }
    }
}
