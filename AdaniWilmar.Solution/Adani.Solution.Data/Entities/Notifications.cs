using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.Data.Entities
{
    public class Notifications : Auditable
    {
        public string Request { get; set; }
        public long RequestId { get; set; }
        public long ReferenceId { get; set; }
        public string Notification { get; set; }
        public long StatusId { get; set; }
    }
}
