using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PackingCostDto
    {
        public long Id { get; set; }
        public long? SkuId { get; set; }
        public List<long> SkuIds { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public long? VerticalId { get; set; }
        public string Vertical { get; set; }

        public long OilTypeId { get; set; }
        public string OilType { get; set; }

        public long PlantId { get; set; }
        public string PlantName { get; set; }

        public decimal ActualPackingCost { get; set; }
        public decimal SalesPackingCost { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public long? SubCategoryId { get; set; }

        public bool IsActive { get; set; }
        public bool IsRasoi { get; set; }

        public int LoginUserId { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
        public bool IsPublished { get; set; }
        public long RoleId { get; set; }
    }
}
