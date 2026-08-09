using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Division : Auditable
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public bool ZPR4 { get; set; }
        public string SalesDocumentType { get; set; }
        public string SalesOrderDocumentType { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }

        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }

    }
}
