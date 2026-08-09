using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class CompetitorDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public string Name { get; set; }

        public long? OilTypeId { get; set; }
        public string OilTypeName { get; set; }

        public long VerticleId { get; set; }
        public string VerticleName { get; set; }
        public string ZoneName { get; set; }
        public long ZoneId { get; set; }
        public string StateName { get; set; }
        public int StateId { get; set; }

        public string DistrictName { get; set; }
        public int DistrictId { get; set; }

        public string CityName { get; set; }
        public int CityId { get; set; }

        public string TerritoryName { get; set; }
        public int TerritoryId { get; set; }

        public string Address { get; set; }

        public string Pincode { get; set; }

        public bool IsActive { get; set; }

        public long LoginUserId { get; set; }


        public List<long> RemoveCompetitorSkuIds { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        //SKU Popup
        public List<long> SelectedSkuIds { get; set; }
        public int SelectedSkuIdsCount { get; set; }
        public string SelecteSkuId { get; set; }

        public string RemovedSkuId { get; set; }
        public List<long> RemovedSkuIds { get; set; }

        public string SelectedOilTypeIdString { get; set; }
        public List<long> SelectedOilTypeIds { get; set; }

        public string MappedSkus { get; set; }

    }

    public class CompetitorSkuInputDto
    {
        public int Id { get; set; }
        public List<long> OilTypeIds { get; set; }
        public long LoginUserId { get; set; }
        public bool IsToReturnInactiveData { get; set; }
        public bool IsToRemoveSelectedIdFromSession { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
