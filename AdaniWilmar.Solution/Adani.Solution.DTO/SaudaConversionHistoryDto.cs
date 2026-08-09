using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaConversionHistoryDto
    {
        public long ConversionId { get; set; }
        public string DealerName { get; set; }
        public string BdoName { get; set; }
        public string ZonalHeadName { get; set; }
        public DateTime ConversionDate { get; set; }
        public string Sku { get; set; }
        public string SaudaQuantity { get; set; }
    }

    public class SaudaConversionHistoryInputDto :LoginUserIdDto
    {
        public long ZoneHeadId { get; set; }
        public long BdoId { get; set; }
        public long StatusId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class SaudaConversionReportInputDto 
    {
        public List<long> StatusIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long VerticalId { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
    }
    }
