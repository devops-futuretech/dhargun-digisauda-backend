using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class QPSSchemeDiscountDto : IAPIInputDTO
    {
        public QPSSchemeDiscountDto() {
            QPSSlabDetails = new List<QPSSlabDetailsDto>();
        }
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long SalesOrgId { get; set; }
        public long LoginUserId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        //public List<long> ZonelistId { get; set; }
        public List<long> OilTypeId { get; set; }
        public List<long> SkuIds { get; set; }
        public List<long> ZoneId { get; set; }
        public List<long> StateId { get; set; }
        //public List<long> StatelistId { get; set; }
        public string StateName { get; set; }
        public string OilTypeName { get; set; }
        public string ZoneName { get; set; }
        //public List<QPSDetails> QPSdiscount { get; set; }
        public List<QPSSlabDetailsDto> QPSSlabDetails { get; set; }
        public bool IsActive { get; set; }
        public  bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public string EncryptedId { get; set; }
        public long SlabCount { get; set; }
        public string SkuName { get; set; }
        public long FromRange { get; set; }
        public long ToRange { get; set; }
        public decimal Discount { get; set; }
        //public long Zone { get; set; }
    }
    public class QPSDetails 
    {
        public long SlabId { get; set; }
        public string SlabName { get; set; }
        public int FromRange { get; set; }
        public int ToRange { get; set; }
        public decimal Discount { get; set; }
    }

    public class QPSListDetails
    {
        public long QPSListId  { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ZoneId { get; set; }
        public long StateId { get; set; }
        public long OilTypeId { get; set; }
    }
}
