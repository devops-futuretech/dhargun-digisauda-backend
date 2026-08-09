using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DateRangeDTO: IAPIInputDTO
    {
        public long Id { get; set; }
        public int FromRange1 { get; set; }
        public int ToRange1 { get; set; }
        public int FromRange2 { get; set; }
        public int ToRange2 { get; set; }
        public int FromRange3 { get; set; }
        public int ToRange3 { get; set; }
        public int FromRange4 { get; set; }
        public int ToRange4 { get; set; }
        public int ToRange5 { get; set; }
        public bool IsActive { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public long LoginUserId { get; set; }

    }
}
