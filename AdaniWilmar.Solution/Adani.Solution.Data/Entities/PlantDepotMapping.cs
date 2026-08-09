using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class PlantDepotMapping : Auditable
    {        
        public long PlantId { get; set; }        
        public long DepotId { get; set; }
    }
}
