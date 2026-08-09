using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class QPSDiscountImportDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long SalesOrgCode { get; set; }
        public long DistributionChannelCode { get; set; }
        public long DivisionCode { get; set; }
        public string OilTypeName { get; set; }
        //public string SkuName { get; set; }
        public string SkuCode { get; set; }
        public string ZoneName { get; set; }
        public string StateName { get; set; }
        public long SlabCount { get; set; }
        public long FromRange { get; set; }
        public long ToRange { get; set; }
        public decimal Discount { get; set; }
        public long OilTypeId { get; set; }
        public long SkuId { get; set; }
        public long ZoneId { get; set; }
        public long StateId { get; set; }
        public long QpsParentId { get; set; }
        public long QpsRowId { get; set; }
        public string Message { get; set; }
    }
}
