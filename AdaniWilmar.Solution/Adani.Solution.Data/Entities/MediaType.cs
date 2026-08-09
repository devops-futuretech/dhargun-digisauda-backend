using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class MediaType : Entity
    {
        [MaxLength(50)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
