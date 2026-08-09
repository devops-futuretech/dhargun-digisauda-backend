using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Month : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
