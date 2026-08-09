using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class DepotDto : UserIdDto
    {
        public DepotDto()
        {
            Areas = new List<AreaDto>();
            Depotlist = new List<DepotDto>();
            Rakelist = new List<DepotDto>();
        }

        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string PinCode { get; set; }

        public string ZoneName { get; set; }
        public long? ZoneId { get; set; }

        public int AreaId { get; set; }
        public string Area { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

        public int DistrictId { get; set; }
        public string District { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }

        public int TerritoryId { get; set; }
        public string TerritoryName { get; set; }

        public long AssociatedPlantId { get; set; }
        public string AssociatedPlantName { get; set; }
        public bool IsActive { get; set; }

        public bool IsToReturnActiveData { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public bool IsChecked { get; set; }

        public List<long> MappedPlantIds { get; set; }
        public List<AreaDto> Areas { get; set; }
        public List<DepotDto> Depotlist { get; set; }
        public List<DepotDto> Rakelist { get; set; }

        public bool IsPlant { get; set; }
        public string Usage { get; set; }

        public string PlantCode { get; set; }
    }

    public class DepotDropDownParam
    {
        public List<long> PlantIds { get; set; }
    }

    public class RakeDto : LoginUserIdDto, IAPIInputDTO
    {
        public RakeDto()
        {
            Areas = new List<AreaDto>();
            Depotlist = new List<DepotDto>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string PinCode { get; set; }

        public string ZoneName { get; set; }
        public long? ZoneId { get; set; }

        public int AreaId { get; set; }
        public string Area { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

        public int DistrictId { get; set; }
        public string District { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }

        public int TerritoryId { get; set; }
        public string TerritoryName { get; set; }

        public long AssociatedPlantId { get; set; }
        public string AssociatedPlantName { get; set; }
        public bool IsActive { get; set; }

        public bool IsToReturnActiveData { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public bool IsChecked { get; set; }

        public List<long> MappedPlantIds { get; set; }
        public List<AreaDto> Areas { get; set; }
        public List<DepotDto> Depotlist { get; set; }
        public List<long> MappedStateIds { get; set; }

        public bool IsPlant { get; set; }
        public string Usage { get; set; }

        public long DepotId { get; set; }
        public string DepotName { get; set; }
        public string DepotCode { get; set; }

        public string AssociatedPlantCodes { get; set; }
        public string AssociatedStates { get; set; }
    }

    public class DepotRakeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string StorageType { get; set; }
    }
    
}
