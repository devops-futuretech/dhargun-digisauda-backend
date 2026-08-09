using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class PackType : Auditable
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string SAPCode { get; set; }
    }
}
