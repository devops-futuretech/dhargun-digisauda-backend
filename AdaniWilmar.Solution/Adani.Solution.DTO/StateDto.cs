using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class StateDto : LoginUserIdDto
    {
       // public int StateId { get; set; }
        public string StateName { get; set; }
        public string EncryptedId { get; set; }
        public bool IsActive { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public List<CityDto> Cities { get; set; }

        public StateDto()
        {
            Cities = new List<CityDto>();
        }
    }
    public class StateOutputDto
    {
        public long LoginUserId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
}
