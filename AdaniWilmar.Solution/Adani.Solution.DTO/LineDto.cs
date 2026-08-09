using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class LineddlDto
    {
        public long LineId { get; set; }
        public string LineName { get; set; }
    }
    public class LineGridDto
    {
        public string EncryptedId { get; set; }
        public long LineId { get; set; }
        public string LineName { get; set; }
        public bool IsActive { get; set; }
        public long Id { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
