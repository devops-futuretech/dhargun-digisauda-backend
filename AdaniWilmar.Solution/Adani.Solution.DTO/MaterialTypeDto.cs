using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class MaterialTypeDto : IAPIInputDTO
    {
        public long VerticalId { get; set; }
        public string VerticalName { get; set; }

        public long SalesOrganizationId { get; set; }
        public string SalesOrganizationName { get; set; }

        public long DistributionChannelId { get; set; }
        public string DistributionChannelName { get; set; }
        public long CreatedBy { get; set; }
        public long LoginUserId { get; set; }
        public string MaterialType { get; set; }
        public bool IsActive { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class MaterialTypesGridDataDto
    {
        public long Id { get; set; }
        public long VerticalId { get; set; }
        public string VerticalName { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganizationName { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannelName { get; set; }
        public string MaterialType { get; set; }
        public bool IsActive { get; set; }
    }

}
