using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ImageNameAddDto
    {
        public long RecordId { get; set; }
        public string Url { get; set; }
        public int PageId { get; set; }
        public long LoginUserId { get; set; }
    }
}
