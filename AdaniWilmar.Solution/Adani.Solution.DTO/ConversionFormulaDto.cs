using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ConversionFormulaDto : IAPIInputDTO
    {
        public long Id { get; set; }
        public long VerticalId { get; set; }
        public long OilTypeId { get; set; }
        public long PackGroupId { get; set; }
        public long SkuId { get; set; }
        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }

        public List<ConversionFormulaDetailsDto> ConversionFormulaDetailsDto { get; set; }
    }

    public class ConversionFormulaDetailsDto
    {
        public long Id { get; set; }
        public long SkuId { get; set; }
        public string Formula { get; set; }
    }

    public class ConversionFormulaGridDto
    {
        public long Id { get; set; }
        public string Vertical { get; set; }
        public string OilType { get; set; }
        public string PackGroup { get; set; }
        public string SkuName { get; set; }
        public bool IsActive { get; set; }
        public List<ConversionFormulaDetailsGridDto> ConversionFormulaDetails { get; set; }
        public ConversionFormulaGridDto()
        {
            ConversionFormulaDetails = new List<ConversionFormulaDetailsGridDto>();
        }
    }

    public class ConversionFormulaDetailsGridDto
    {
        public long Id { get; set; }        
        public long SkuId { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string Formula { get; set; }
    }

    public class BaseSkuInputDto
    {
        public long Id { get; set; }        
        public long OilTypeId { get; set; }
        public long PackGroupId { get; set; }
    }
}
