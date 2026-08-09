using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Benefits : Auditable
    {
        public long BenefitTypeId { get; set; }      
        [MaxLength(150)]
        public string BenefitCategory { get; set; }
        //public long BenefitDays { get; set; }       
        public bool IsActive { get; set; }

        public virtual BenefitTypes BenefitType { get; set; }
    }
}
