using System.ComponentModel.DataAnnotations;


namespace Adani.Solution.Data.Entities
{
    public class SaudaConversionType :Auditable
    {
        [MaxLength(150)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
