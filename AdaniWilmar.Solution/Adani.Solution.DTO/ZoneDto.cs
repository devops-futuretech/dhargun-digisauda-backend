using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class RegionDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class ZoneDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public string Name { get; set; }
        public bool isActive { get; set; } 
        public string States { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }


    public class ZoneMappingDto : ZoneDto
    {
        public List<StateDto> States { get; set; } 
    }

    public class CheckBoxDto  
    {
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public bool Checked
        {
            get;
            set;
        }
    }

    public class AddorUpdateZoneDto : UserIdDto,IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public string Name { get; set; }
        public bool isActive { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public List<CheckBoxDto> States { get; set; }
    }

}