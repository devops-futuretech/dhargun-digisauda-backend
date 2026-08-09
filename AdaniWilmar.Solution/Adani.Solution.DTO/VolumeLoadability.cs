using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class VolumeLoadability :IAPIInputDTO
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long PlantId { get; set; }
        public string Plant { get; set; }
        public List<long> SkuIds { get; set; }
        public string Sku { get; set; }
        public decimal MaxAllowableSingleSku { get; set; }
        public decimal MaxAllowableMultipleSku { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public decimal VehicleSize { get; set; }
    }

    public class VolumeLoadabilityGridDataDto
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long PlantId { get; set; }
        public string Plant { get; set; }
        public List<long> SkuIds { get; set; }
        public string Sku { get; set; }
        public decimal VehicleSize { get; set; }
        public decimal MaxAllowableSingleSku { get; set; }
        public decimal MaxAllowableMultipleSku { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }
        public string SkuCode { get; set; }
    }

    public class VolumeLoadabilityUploadDto : CommonResultDto
    {
        public string SkuCode { get; set; }
        public string PlantCode { get; set; }
        public decimal MaxAllowableSingleSku { get; set; }
        public decimal MaxAllowableMultipleSku { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string IsActive { get; set; }
        public long CreatedBy { get; set; }
        public decimal VehicleSize { get; set; }
    }
}
