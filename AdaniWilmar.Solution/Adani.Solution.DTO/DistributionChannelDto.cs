using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    
    public class DistributionChannelDto:IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long LoginUserId { get; set; }
        public string Name { get; set; }
        public string SAPCode { get; set; }
        public bool IsActive { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganization { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
    public class DistributionChannelExportDto
    {
        public string SalesOrganization { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }

    public class DistributionChannelddlDto
    {
        public long Id { get; set; }
        public string DistributionChannelName { get; set; }
    }

    public class DistributionChannelUploadDto: CommonResultDto
    {
        public string SalesOrganizationCode { get; set; }
        public long LoginUserId { get; set; }
        public string Name { get; set; }
        public string SAPCode { get; set; }
        public string IsActive { get; set; }
    }
}
