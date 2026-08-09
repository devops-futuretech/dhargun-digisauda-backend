using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SAPSkuDto
    {
        public string SkuCode { get; set; }
        public string MaterialDescription { get; set; }
        public string UOM { get; set; }
        public string VerticalGroupCode { get; set; }
        public string SalesDivision { get; set; }
        public string MaterialGroup1 { get; set; }
        public string OilTypeCode { get; set; }
        public string VerticalCode { get; set; }
        public string PackTypeCode { get; set; }
        public string ConvertionType { get; set; }
        public decimal ConvertionFactor { get; set; }
        public string PackGroups { get; set; }
        public string MaterialType { get; set; }

    }
    public class HANASAPSku
    {
        public List<HANASAPSkuDto> SkuList { get; set; }

        public HANASAPSku()
        {
            SkuList = new List<HANASAPSkuDto>();
        }
    }
    public class HANASAPSkuDto
    {
        public string SkuCode { get; set; }
        public string MaterialDescription { get; set; }       
        public string VerticalGroupCode { get; set; }
        public string SalesDivision { get; set; }
        public string MaterialGroup1 { get; set; }
        public string OilTypeCode { get; set; }
        public string VerticalCode { get; set; }
        public string PackTypeCode { get; set; }
        public string ConvertionType { get; set; }
        public string PackGroups { get; set; }
        public string MaterialType { get; set; }
        public bool IsBaseSku { get; set; }
        public bool IsRequiredToAttachTT { get; set; }
        public string SalesDocumentType { get; set; }
        public string DocumentType { get; set; }
        public decimal ProcessCost { get; set; }
        public decimal PackSizeQuantity { get; set; }
        public string PackSize { get; set; }
        public string SubCategory { get; set; }
        public decimal PremiumAmount { get; set; }
        public string StorageLocation { get; set; }
        public List<SkuConvertionFactor> SkuConvertionFactor { get; set; }

        public HANASAPSkuDto()
        {
            SkuConvertionFactor = new List<SkuConvertionFactor>();
        }
    }

   public class SkuConvertionFactor
    {
        public decimal ConvertionFactor { get; set; }
        public string UOM { get; set; }
    }
}
