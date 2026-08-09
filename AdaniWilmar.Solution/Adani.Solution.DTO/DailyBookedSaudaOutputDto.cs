using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DailyBookedSaudaOutputDto
    {
        public DateTime BookedDate { get; set; }
        public string PartyName { get; set; }
        public string BDOName { get; set; }
        public string ZonalTrader { get; set; }
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public long ProductGroupId { get; set; }
        public string ProductGroup { get; set; }
        public decimal QuantityInMT { get; set; }
        public decimal QuantityCase { get; set; }
        public long UserId { get; set; }
        public long StateId { get; set; }
        public string SaleDocumentType { get; set; }
        public string MaterialType { get; set; }
        public string SkuName { get; set; }
        public long SkuId { get; set; }
        public string StateName { get; set; }
        public long OilPackGroupType { get; set; }
        public List<SkuListReportDto> SkuListReportDto { get; set; }
    }

    public class StateList
    {
        public long StateId  { get; set; }
        public string StateName { get; set; }
        public List<OilTypeList> OilTypes { get; set; }
        public decimal QuantityInMT { get; set; }
        public decimal QuantityCase { get; set; }
    }

    public class SaudaReportDtoNH
    {
        public List<StateList> StateList { get; set; }
        public decimal QuantityInMT { get; set; }
        public decimal QuantityCase { get; set; }
    }

    public class OilTypeList
    {
        public long OilTypeId { get; set; }
        public string OilType { get; set; }
        public List<SkuListReportDto> SkuListReportDto { get; set; }
        public decimal QuantityInMT { get; set; }
        public decimal QuantityCase { get; set; }
    }
    public class SkuListReportDto
    {
        public string SkuName { get; set; }
        public decimal BidQuantity { get; set; }
        public decimal BidQuantityCase { get; set; }
    }

    public class BookedSaudaOutputDto
    {
        public long OilTypeId { get; set; }
        public long PackGroupId { get; set; }
        public string OilType { get; set; }
        public decimal QuantityInMT { get; set; }
        public decimal premiumquantityInMT { get; set; }
        public decimal BakeryquantityInMT { get; set; }
        public decimal LauricquantityInMT { get; set; }
        public decimal PopularquantityInMT { get; set; }
        public string MaterialType { get; set; }
        public decimal QuantityCase { get; set; }
        public decimal PremiumQuantityCase { get; set; }
        public decimal BakeryQuantityCase { get; set; }
        public decimal LauricQuantityCase { get; set; }
        public decimal PopularQuantityCase { get; set; }
    }
}
