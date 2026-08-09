using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PercetileNumberDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public List<long> OilTypeIds { get; set; }
        public string OilType { get; set; }
        public List<long> PackGroupIds { get; set; }
        public string PackGroup { get; set; }
        public long PercentileNumbers { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsActive { get; set; }

        public List<PercetileNumberDetailsDto> PercetileNumberDetails { get; set; }
        public PercetileNumberDto()
        {
            PercetileNumberDetails = new List<PercetileNumberDetailsDto>();
        }


    }
    public class PercetileNumberDetailsDto : KendoGridResult
    {
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long PackGroupId { get; set; }
        public string PackGroup { get; set; }
    }
}
