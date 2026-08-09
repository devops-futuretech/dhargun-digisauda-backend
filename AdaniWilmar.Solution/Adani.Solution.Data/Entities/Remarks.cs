using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Remarks : Auditable
    {
        [Required]
        public long TableId { get; set; }
        public string TableName { get; set; }
        public int ReasonTypeId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
