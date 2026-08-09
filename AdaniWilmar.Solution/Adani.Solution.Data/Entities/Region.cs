using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Region : Entity
    {
        public string RegionName { get; set; }

        [MaxLength(150)]
        public string TamilName { get; set; }

        public int? SortOrder { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
