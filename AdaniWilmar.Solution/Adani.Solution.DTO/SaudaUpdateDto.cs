using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaUpdateDto : IAPIInputDTO
    {
        public long SaudaId { get; set; }
        public long OilTypeId { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public long ModifiedBy { get; set; }
        public List<long> SaudaOrderIds { get; set; }
        public List<string> EncryptedIds { get; set; }
        public long SaudaOrderId { get; set; }
        public long VerticalId { get; set; }
        public long RoleId { get; set; }
        public long LoginUserId { get; set; }
        public List<long> StateIds { get; set; }
        public SaudaUpdateDto()
        {
            SaudaOrderIds = new List<long>();
            EncryptedIds = new List<string>();
            SaudaModificationIds = new List<long>();
        }
        public long? ZoneId { get; set; }
        public string Zone { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }

        public int DistrictId { get; set; }
        public string District { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public List<long> SaudaModificationIds { get; set; }
        public List<SaudaSkuQuantityDto> SkuList { get; set; }

    }

    public class SaudaApproveInputDto : LoginUserIdDto
    {
        public long SaudaOrderId { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
    }

    public class SaudaConversionReprocessDto : IAPIInputDTO
    {
        public List<long> SaudaConversionIds { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long ModifiedBy { get; set; }
    }

    public class SaudaExtensionReprocessDto : IAPIInputDTO
    {
        public long ModifiedBy { get; set; }
        public bool IsReprocess { get; set; }
        public List<long> SaudaExtensionIds { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class LiftingRequestReprocessDto : IAPIInputDTO
    {
        public long ModifiedBy { get; set; }
        public bool IsReprocess { get; set; }
        public List<long> LiftingIds { get; set; }
        public List<string> EncryptedIds { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
    public class SaudaSkuQuantityDto
    {
        public long SkuId { get; set; }
        public decimal Quantity { get; set; }
    }

}
