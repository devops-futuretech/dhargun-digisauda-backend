using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.Solution.DTO;

namespace Adani.Solution.MVC.Models
{
    public class SaudaConversionUnitAndDiffRateModel : SaudaConversionUnitAndDifferenceRateAddDto, IAPIInputDTO
    {
        public long VerticalId { get; set; }
        public long OilTypeId { get; set; }
        public long ToPackGroupId { get; set; }
        public long ToSkuId { get; set; }
        public bool SaveAnother { get; set; }
        public decimal ToUnit { get; set; }
        public decimal BasicRate { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class SaudaConversionUnitAndDiffRateUploadDto : CommonResultDto
    {
        public string OilType { get; set; }
        public string FromPackGroup { get; set; }
        public string FromSku { get; set; }
        public decimal FromUnit { get; set; }
        public string FromSkuCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long CreatedBy { get; set; }
        public string PlantOrDepot { get; set; }
        public string State { get; set; }
        public string ToPackGroup { get; set; }
        public string ToSku { get; set; }
        public decimal ToUnit { get; set; }
        public string ToSkuCode { get; set; }
        public decimal BasicRate { get; set; }
        public string IsActive { get; set; }
    }
    }