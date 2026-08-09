using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adani.Solution.DTO;

namespace Adani.Solution.Data.Entities
{
    public class PercentileNumber : Auditable
    {
        
        public long DivisionId { get; set; }
        public long PercentileNumbers { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public virtual Division Division { get; set; }
       

        
    }
    public class PercentileNumberDetails : Auditable
    {
        public long PackGroupId { get; set; }
        public long OilTypeId { get; set; }
        public long PercentileNumberId { get; set; }
        public virtual PercentileNumber PercentileNumber { get; set; }
        public virtual OilType OilType { get; set; }
        public virtual PackGroup PackGroup { get; set; }
    }
}
