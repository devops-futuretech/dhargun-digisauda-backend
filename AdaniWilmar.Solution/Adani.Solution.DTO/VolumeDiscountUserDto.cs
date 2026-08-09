using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class VolumeDiscountUserDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VolumeDiscountHistoryId { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public List<long> OilTypeIds { get; set; }
        public List<long> OilPackingTypeIds { get; set; }  /* BPOrCPWise - PackGroup - OilPackingTypeId*/
        public List<long> CustomerGroupIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> BDOIds { get; set; }
        public List<long> SkuIds { get; set; }
        public bool IsEdit { get; set; }
        public bool IsActive { get; set; }

        public decimal StartVolumeSlabInMT { get; set; }
        public decimal EndVolumeSlabInMT { get; set; }
        public long VolumeSlabCount { get; set; }

        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<VolumeDiscountSlabDto> VolumeSlabsList { get;set;}
        public List<VolumeDiscountUserMappingDto> VolumeDiscountUserMappingDto { get; set; }

        public VolumeDiscountUserDto()
        {
            VolumeSlabsList = new List<VolumeDiscountSlabDto>();
            VolumeDiscountUserMappingDto = new List<VolumeDiscountUserMappingDto>();
        }
    }
}
