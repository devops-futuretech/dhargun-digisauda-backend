using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class ContractTypeDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public int SelectedTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class DeliveryTypeDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long SelectedTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class OilTypeDto : LoginUserIdDto,IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long VerticalId { get; set; }
        public string VerticalName { get; set; }
        public decimal LitreConversion { get; set; }
        //public bool IsRasoi { get; set; } 
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public decimal VolumeCapacity { get; set; }

        public long SelectedOilTypeId { get; set; }

        public long SalesOrganizationId { get; set; }
        public string SalesOrganizationName { get; set; }

        public long DistributionChannelId { get; set; }
        public string DistributionChannelName { get; set; }



    }

    public class DeliveryTypeInputDto : LoginUserIdDto
    {
        public long SelectedTypeId { get; set; }
    }

    public class ContractTypeInputDto : LoginUserIdDto
    {
        public long SelectedTypeId { get; set; }
    }

    public class OilTypeInputDto
    {
        public long VerticalId { get; set; }
    }

    public class SkuInputDto : LoginUserIdDto
    {
        public long OilTypeId { get; set; }
        public long DealerId { get; set; }
        public long PlantOrDepotId { get; set; }
        public long EmployeeDiscountParentId { get; set; }
        public decimal VehicleSize { get; set; }
        public List<long> SkuIds { get; set; }
        public long PlantId { get; set; }
    }

    public class IngredientDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long LoginUserId { get; set; }
    }
}
