using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class UserAttendance : Auditable
    {
        [Required]
        public long UserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LoginTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LogoutTime { get; set; }

        public virtual User User { get; set; }
    }
}
