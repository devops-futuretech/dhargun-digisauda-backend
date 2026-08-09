using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class BdoCompetitor : Auditable
    {
        [Required]
        public string Name { get; set; }

        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        public int UserType { get; set; }
        public long DealerId { get; set; }

        public long BdoWholesellerId { get; set; }
    }
}
