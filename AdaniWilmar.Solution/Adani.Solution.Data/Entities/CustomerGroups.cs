using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class CustomerGroups : Auditable
    {
        public CustomerGroups()
        {
            this.CustomerGroupDetails = new HashSet<CustomerGroupDetails>();
        }
        [Required]
        public string Name { get; set; }
        public long SalesOrganizationId { get; set; }
        public long DistributionChannelId { get; set; }
        public long DivisionId { get; set; }
        public bool IsActive { get; set; }
        public bool IsBaseGroup { get; set; }

        public virtual SalesOrganization SalesOrganization { get; set; }
        public virtual DistributionChannel DistributionChannel { get; set; }
        public virtual Division Division { get; set; }
        public virtual ICollection<CustomerGroupDetails> CustomerGroupDetails { get; set; }
    }


    public class CustomerGroupMappings : Auditable
    {
        public long CustomerGroupId { get; set; }
        public long DerivedCustomerGroupId { get; set; }
        public bool IsActive { get; set; }
        public virtual CustomerGroups CustomerGroup { get; set; }
    }

    }
