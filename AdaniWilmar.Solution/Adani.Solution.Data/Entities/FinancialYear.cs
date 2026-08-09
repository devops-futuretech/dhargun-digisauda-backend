using System.ComponentModel.DataAnnotations;
using System;
namespace Adani.Solution.Data.Entities
{
    public class FinancialYear : Auditable
    {
        [Required]
        public string Year { get; set; }
        [Required]
        public DateTime EffectiveFrom { get; set; }
        [Required]
        public DateTime EffectiveTo { get; set; }
        public bool IsActive { get; set; }
    }
}
