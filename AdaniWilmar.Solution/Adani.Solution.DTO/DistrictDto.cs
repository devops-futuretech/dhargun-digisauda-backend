using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class DistrictDto
    {
        public int DistrictId { get; set; }
        public string EncryptedId { get; set; }
        public string DistrictName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int TerritoryId { get; set; }
        public string TerritoryName { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class TerritoryDto : IAPIInputDTO
    {
        public TerritoryDto()
        {
            District = new List<CheckBoxDto>();
        }
        public int Id { get; set; }
        public string TerritoryName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<CheckBoxDto> District { get; set; }
        public List<DistrictDto> DistrictList { get; set; }
    }

    public class TerritoryDistrictParam
    {
        public int Id { get; set; }
        public bool IsToReturnInactiveData { get; set; }
    }
}
