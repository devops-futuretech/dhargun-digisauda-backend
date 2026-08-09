using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SupportAttachmentDto
    {
        public long MediaId { get; set; }
        public long SupportId { get; set; }
        public long ConsentImageId { get; set; }
        public string FileName { get; set; }
        public string MediaPath { get; set; }
        public string MediaFullPath { get; set; }
        public int? MediaTypeId { get; set; }
        public string MediaType { get; set; }
        public string FileExtension { get; set; }
        //public Stream FileInputStream { get; set; }
        public byte[] FileByteArray { get; set; }
    }
}
