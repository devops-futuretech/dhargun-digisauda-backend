using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adani.Solution.Data.Entities
{
    public class Depot : Auditable
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        //public long? ZoneId { get; set; }
        //public int StateId { get; set; }
        //public int DistrictId { get; set; }
        //public int CityId { get; set; }
        [MaxLength(10)]
        public string Pincode { get; set; }
        [MaxLength(4000)]
        public string Location { get; set; }
        public int StorageTypeId { get; set; }
        public string Usage { get; set; }

        public bool IsActive { get; set; }
        public bool IsPlant { get; set; }
        public bool IsSAPData { get; set; }
        public bool IsSAPDataSyncOrNot { get; set; }

        public long DepotId { get; set; }
        public string MappedStateId { get; set; }

        //public virtual Zone Zone { get; set; }
        //public virtual State State { get; set; }
        //public virtual District District { get; set; }
        //public virtual City City { get; set; }

    }
}
