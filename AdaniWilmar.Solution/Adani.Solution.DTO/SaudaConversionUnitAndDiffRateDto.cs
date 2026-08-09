using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConversionUnitAndDiffRateDto
    {
        public long ConversionId { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long FromPackGroupId { get; set; }
        public string FromPackGroup { get; set; }
        public long FromSkuId { get; set; }
        public string FromSku { get; set; }
        public string FromSkuCode { get; set; }
        public long ToPackGroupId { get; set; }
        public string ToPackGroup { get; set; }
        public long ToSkuId { get; set; }
        public string ToSku { get; set; }
        public string ToSkuCode { get; set; }
        public decimal Unit { get; set; }
        public decimal BasicRate { get; set; }
        public decimal FromUnit { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string ValidFromInString { get; set; }
        public string ValidToInString { get; set; }
        public bool IsActive { get; set; }
        public long SourceId { get; set; }
        public long StateId { get; set; }
        public string State { get; set; }
        public string Source { get; set; }
    }

    public class SaudaConversionUnitAndDiffRateInputDto :LoginUserIdDto 
    {
        public long OilTypeId { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
    }

    public class SaudaConversionUnitAndDiffRateExportDto
    {
        public string FromPackGroup { get; set; }
        public string FromSku { get; set; }
        public string FromSkuCode { get; set; }
        public string FromUnit { get; set; }
        public string ToPackGroup { get; set; }
        public string ToSku { get; set; }
        public string ToSkuCode { get; set; }
        public string ToUnit { get; set; }
        public decimal BaseRate { get; set; }
        public string Source { get; set; }
        public string State { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public bool IsActive { get; set; }
    }
}
