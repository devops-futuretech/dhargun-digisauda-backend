using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class VolumeDiscountDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VerticalId { get; set; }
        public long CustomerGroupId { get; set; }
        public string CustomerGroup { get; set; }

        public long OilTypeId { get; set; }
        public string OilTypeName { get; set; }
        public long SkuId { get; set; }
        public List<long> SkuIds { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public long ParentId { get; set; }
        public decimal ActualDiscount { get; set; }
        public long VolumeSlabCount { get; set; }
        public long? SubCategoryId { get; set; }

        //BPOrCPWise
        public long OilPackingTypeId { get; set; }
        public string OilPackingType { get; set; }

        public List<long> ZoneId { get; set; }
        public List<long> StateId { get; set; }
        public List<long> TerritoryId { get; set; }
        public List<long> DistrictId { get; set; }
        public List<long> CityId { get; set; }

        public List<DiscountSkuCityMappingDto> Cities { get; set; }

        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public VolumeDiscountDto()
        {
            Cities = new List<DiscountSkuCityMappingDto>();
        }
    }
}
