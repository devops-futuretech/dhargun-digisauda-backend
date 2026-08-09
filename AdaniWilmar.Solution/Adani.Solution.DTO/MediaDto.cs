using System.Collections.Generic;
using System.IO;

namespace Adani.Solution.DTO
{
    public class MediaDto
    {
        public long MediaId { get; set; }
        public string ProfileOrMediaSuffix { get; set; }
        public string ProfileOrMediaContainer { get; set; }
        public Stream Stream { get; set; }
        public string FileExtension { get; set; }
        public IDictionary<string, string> MetadataList { get; set; }
        public bool IsThumbnail { get; set; }
        public string BlobName { get; set; }
        public string FileName { get; set; }

        public int MediaTypeId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
