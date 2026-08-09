using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class QpsDiscountUploadDto 
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public long SalesOrgCode { get; set; }

        public long DistributionChannelCode { get; set; }

        public long DivisionCode { get; set; }

        public string OilTypeId { get; set; }

        public string OilTypeName { get; set; }

        //public string SkuName { get; set; }
        public string SkuCode { get; set; }

        public string ZoneName { get; set; }

        public string StateName { get; set; }

        public string SkuId { get; set; }

        public string ZoneId { get; set; }

        public string StateId { get; set; }

        public long SlabCount { get; set; }

        public int FromRange { get; set; }

        public int ToRange { get; set; }

        public decimal Discount { get; set; }

        public long QpsParentId { get; set; }

        public long QpsRowId { get; set; }

        public long CreatedBy { get; set; }

        public long UpdateBy { get; set; }

        public string Message { get; set; }

        //public bool PostStatus { get; set; }

        //public string PostMessage { get; set; }
    }
}
