using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PrimaryFreightDto
    {
        public long Id { get; set; }
        public long? PlantId { get; set; }
        public string PlantName { get; set; }
        public long DepotId { get; set; }
        public string DepotName { get; set; }
        public string DepotCode { get; set; }
        public long TransportModeId { get; set; }
        public string TransportMode { get; set; }
        public decimal LoadCapacity { get; set; }
        public decimal HireCost { get; set; }
        public long VerticalId { get; set; }
        public string Vertical { get; set; }
        public decimal ActualFreight { get; set; }
        public decimal SalesFreight { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsPublished { get; set; }
        public long RoleId { get; set; }
    }
}
