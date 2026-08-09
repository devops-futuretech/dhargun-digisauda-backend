using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class RaNotification : Auditable
    {
        public bool SMS { get; set; }

        public bool Email { get; set; }

        public bool InAppNotification { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ValidTo { get; set; }
        public string CautionNotificationTimes { get; set; }
    }
}
