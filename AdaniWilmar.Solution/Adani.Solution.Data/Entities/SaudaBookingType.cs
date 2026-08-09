using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class SaudaBookingType : Auditable
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
