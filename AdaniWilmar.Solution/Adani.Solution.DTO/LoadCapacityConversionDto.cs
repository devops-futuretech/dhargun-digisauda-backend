using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LoadCapacityConversionDto
    {
        public long Id { get; set; }
        public long TransportModeId { get; set; }
        public string TransportMode { get; set; }
        public decimal LoadCapacity { get; set; }
        public long SkuId { get; set; }
        public List<long> SkuIds { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public decimal LoadQuantity { get; set; }
        public long? VerticalId { get; set; }
        public string Vertical { get; set; }
        public long? OilTypeId { get; set; }
        public string OilType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsPublished { get; set; }

        public long? SubCategoryId { get; set; }
        public decimal ActualLoadQuantity { get; set; }
        public long RoleId { get; set; }
    }
}
