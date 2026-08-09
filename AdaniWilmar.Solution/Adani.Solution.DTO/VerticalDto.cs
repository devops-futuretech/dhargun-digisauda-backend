using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class VerticalDto : UserIdDto,IAPIInputDTO
    {
        public long Id { get; set; }
        public String EncryptedId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CCArea { get; set; }
        public long OilTypeId { get; set; }
        public long VerticalId { get; set; }
        public bool IsActive { get; set; }
        public bool ZPR4 { get; set; }
        public bool IsToReturnActiveData { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long SalesOrganizationId { get; set; }
        public string SalesOrganizationName { get; set; }
        public string SalesDocumentType { get; set; }
        public string SalesOrderDocumentType { get; set; }
        public long DistributionChannelId { get; set; }
        public string DistributionChannelName { get; set; }
    }

    public class DivisionUploadDto:CommonResultDto
    {
        public long LoginUserId { get; set; }
        public string SalesOrganizationCode { get; set; }
        public string DistributionChannelCode { get; set; }
        public string SalesDocumentType { get; set; }
        public string SalesOrderDocumentType { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string IsActive { get; set; }
        public string ZPR4 { get; set; }
    }
}
