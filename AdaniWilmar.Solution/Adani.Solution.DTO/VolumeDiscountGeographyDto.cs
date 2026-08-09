using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class VolumeDiscountGeographyDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VolumeDiscountHistoryId { get; set; }
        public long VerticalId { get; set; }
        public long DiscountType { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> OilPackingTypeIds { get; set; }  /* BPOrCPWise - PackGroup - OilPackingTypeId*/
        public List<long> CustomerGroupIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> SkuIds { get; set; }
        public List<int> CityIds { get; set; }
        public List<long> ZoneIds { get; set; }
        public List<int> StateIds { get; set; }
        public List<int> DistrictIds { get; set; }
        public List<int> TerritoryIds { get; set; }

        public decimal StartVolumeSlabInMT { get; set; }
        public decimal EndVolumeSlabInMT { get; set; }
        public long VolumeSlabCount { get; set; }
        public decimal Discount { get; set; }
        public string Vertical { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public bool IsEdit { get; set; }
        public bool IsActive { get; set; }
        public List<VolumeDiscountSlabDto> VolumeSlabsList { get; set; }
        public List<VolumeDiscountGeographyMappingDto> GeographyBasedVolumeDiscountDetails { get; set; }

        public VolumeDiscountGeographyDto()
        {
            VolumeSlabsList = new List<VolumeDiscountSlabDto>();
            GeographyBasedVolumeDiscountDetails = new List<VolumeDiscountGeographyMappingDto>();
        }
    }
}
