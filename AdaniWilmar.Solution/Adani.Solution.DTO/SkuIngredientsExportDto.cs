using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
   public class SkuIngredientsExportDto
    {
        public long SkuIngrediantId { get; set; }
        public long SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string IngredientName { get; set; }       
        public string OilTypeName { get; set; }
        public string VerticleName { get; set; }
        public decimal Percentage { get; set; }
        public string PlantName { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public string Ingredients { get; set; }

        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
    }
}
