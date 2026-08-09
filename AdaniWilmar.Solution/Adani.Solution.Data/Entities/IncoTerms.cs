using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class IncoTerms : Auditable
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int Type { get; set; }
        public string SAPName { get; set; }
    }
}
