using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SalesOrganizationDto:IAPIInputDTO
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long LoginUserId { get; set; }
        public string Name { get; set; }
        public string SAPCode { get; set; }
        public bool IsActive { get; set; }
        public string PostMessage { get; set; }
        public bool PostStatus { get; set; }
    }
    public class SalesOrganizationExportDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }

    public class SalesOrganizationddlDto
    {
        public long Id { get; set; }
        public string SalesOrganizationName { get; set; }


    }

    public class SalesOrganizationUploadDto: CommonResultDto
    {
        public long LoginUserId { get; set; }
        public string Name { get; set; }
        public string SAPCode { get; set; }
        public string IsActive { get; set; }
    }


}
