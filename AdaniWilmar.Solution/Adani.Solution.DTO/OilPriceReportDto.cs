using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.DTO
{
    public class OilPriceReportInputDto
    {
        public long LoginUserId { get; set; }
        public long SaudaBookingTypeId { get; set; }
        public long OilTypeId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public long TransportModeId { get; set; }
        public long OilPackingTypeId { get; set; }
        public long PlantId { get; set; }
        public long DepotId { get; set; }
        public long FreightZoneId { get; set; }
        public long FreightRouteId { get; set; }
        public long VerticalId { get; set; }
        public long IncoTermId { get; set; }
        public long[] SkuId { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
    }

    public class ReportInputDto
    {
        public long VerticalId { get; set; }
        public CostType CostType { get; set; }
        public long LoginUserId { get; set; }
        public long OilTypeId { get; set; }
        public List<long> SkuId { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        public List<long> NationalHeadIds { get; set; }
        public List<long> ZonalHeadIds { get; set; }
        public List<long> BdoIds { get; set; }
        public List<long> DealerIds { get; set; }
        public int PageNo { get; set; }
    }

    public enum CostType
    {
        PackingCost = 1,
        HoneyCombCost = 2,
        SchemeCost=3,
        CushionMarginCost= 4  
    }

}
