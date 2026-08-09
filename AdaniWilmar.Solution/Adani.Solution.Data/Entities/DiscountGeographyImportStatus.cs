using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Data.Entities
{
    public class DiscountGeographyImportStatus : Entity
    {
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        public string MaterialCode { get; set; }
        public string DiscountReason { get; set; }
        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long LoginUserId { get; set; }
        public string Zone { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }        
        public string OilType { get; set; }
        public string PackGroup { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; }
        public string PackType { get; set; }        
    }
}
