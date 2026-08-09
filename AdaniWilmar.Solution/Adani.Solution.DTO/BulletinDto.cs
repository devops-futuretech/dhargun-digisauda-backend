using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class BulletinDto : LoginUserIdDto
    {
        public long BulletinId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public int ContentTypeId { get; set; }
        public long? ReviewedBy { get; set; }
        public bool? IsApproved { get; set; }
        public int ImageCount { get; set; }
        public string FileDetail { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public bool IsEdit { get; set; }
        public List<BulletinMediaDto> MediaList { get; set; }

        public BulletinDto()
        {
            MediaList = new List<BulletinMediaDto>();
        }
    }
}

namespace Adani.Solution.DTO
{
    public class BulletinMediaDto
    {
        public string MediaPath { get; set; }
        public string MediaTypeName { get; set; }
        public int MediaTypeId { get; set; }
        public long BulletinMediaId { get; set; }
    }
}

namespace Adani.Solution.DTO
{
    public class BulletinOutputDto
    {
        public int ContentTypeId { get; set; }
        public string ContentTypeName { get; set; }

        public List<BulletinDto> Bulletin { get; set; }

        public BulletinOutputDto()
        {
            Bulletin = new List<BulletinDto>();
        }
    }
}
