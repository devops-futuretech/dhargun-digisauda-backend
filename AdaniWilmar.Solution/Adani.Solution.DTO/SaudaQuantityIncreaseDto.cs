using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaQuantityIncreaseListDto
    {
        public long Id { get; set; }
        public string Vertical { get; set; }
        public string OilType { get; set; }
        public string PackingType { get; set; }
        public decimal MaximumPercentageQtyIncrease { get; set; }
        public bool IsActive { get; set; }
    }

    public class SaudaQuantitySaveDto : IAPIInputDTO
    {
        public SaudaQuantitySaveDto()
        {
            SaudaQuantityList = new List<SaudaQuantitySaveInputDto>();
        }
        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<SaudaQuantitySaveInputDto> SaudaQuantityList { get; set; }
    }

    public class SaudaQuantitySaveInputDto
    {
        public long Id { get; set; }

        public long VerticalId { get; set; }
        public string Vertical { get; set; }

        public long OilTypeId { get; set; }
        public string OilType { get; set; }

        public long PackGroupId { get; set; }
        public string PackGroup { get; set; }

        public decimal MaximumPercentageQtyIncrease { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class RaSaudaAllocationListDto : IAPIInputDTO
    {
        public long Id { get; set; }

        public decimal GuaranteePricePercentage { get; set; }

        public TimeSpan SaudaAllocationTime { get; set; }

        public DateTime SaudaAllocationTimeView { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SaudaAllocationTimeString { get; set; }

        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
