using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
     public class RAMaterialCostDto
    {
        public long Id { get; set; }
        public long PlantId { get; set; }
        public string PlantName { get; set; }
        public long? VerticalId { get; set; }
        public long RoleId { get; set; }
        public string Vertical { get; set; }
        //OilWise
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public decimal RateOrMT { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsPublished { get; set; }
    }
}
