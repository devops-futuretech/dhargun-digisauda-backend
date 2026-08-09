using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adani.Solution.Data.Enum;

namespace Adani.Solution.Data.Entities
{
    public class Audit : EntityLong
    {
        [Required]
        [MaxLength(20)]
        public string Resource { get; set; }


        [Required]
        public ActionType Action { get; set; }

        [Required]
        public string Changeset { get; set; }

        public long? PerformedBy { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime PerformedDate { get; set; }
    }
}
