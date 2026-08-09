using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class SupportAttachment : Auditable
    {
        public long SupportId { get; set; }
        public string FileName { get; set; }
        public string MediaPath { get; set; }
        public int? MediaTypeId { get; set; }

        public virtual Support Support { get; set; }
        public virtual MediaType MediaType { get; set; }
    }
}
