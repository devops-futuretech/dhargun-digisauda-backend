using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class IngredientsDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Isactive { get; set; }
    }

    public class IngredientsUploadDto : CommonResultDto
    {
        public string Name { get; set; }
        public string Vertical { get; set; }
        public string IsActive { get; set; }
    }

    public class IngredientCostDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long IngredientId { get; set; }
        public string Vertical { get; set; }
        public string IngredientName { get; set; }
        public decimal LooseOilRate { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsPublished { get; set; }

        public long PlantId { get; set; }
        public string PlantName { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class IngredientCostUploadDto : CommonResultDto
    {
        public string PlantCode { get; set; }
        public string Vertical { get; set; }
        public string IngredientName { get; set; }
        public decimal LooseOilRate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long CreatedBy { get; set; }
    }

    public class SkuIngredientUploadDto : CommonResultDto
    {
        public string Vertical { get; set; }
        public string Ingredients { get; set; }
        public string OilType { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string PlantCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long CreatedBy { get; set; }
    }

    public class SkuIngredientDto : LoginUserIdDto
    {
        public SkuIngredientDto()
        {
            SkuIngredientPercentage = new List<SkuIngredientPercentage>();
            SkuIds = new List<long>();
        }

        public long Id { get; set; }
        public long IngredientId { get; set; }

        public string IngredientName { get; set; }
        public long SkuId { get; set; }
        public IList<long> SkuIds { get; set; }
        public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public long? OilTypeId { get; set; }
        public string OilTypeName { get; set; }

        //public long VerticleId { get; set; }
        public string VerticleName { get; set; }

        public decimal Percentage { get; set; }

        public long SkuIngrediantPlantId { get; set; }
        public long PlantId { get; set; }
        public string Plant { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<SkuIngredientPercentage> SkuIngredientPercentage { get; set; }

    }

    public class SkuIngredientPercentage : LoginUserIdDto
    {
        public SkuIngredientPercentage()
        {
            Ingredients = new IngredientDownDto();
        }
        public long Id { get; set; }
        public decimal Percentage { get; set; }

        [UIHint("IngredientNamePartial")]
        public IngredientDownDto Ingredients { get; set; }
    }

    public class IngredientDownDto
    {
        public long IngredientId { get; set; }
        public string IngredientName { get; set; }
    }
}
