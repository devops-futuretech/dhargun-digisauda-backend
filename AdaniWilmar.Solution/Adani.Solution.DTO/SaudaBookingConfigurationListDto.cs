using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaBookingConfigurationListDto
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public List<long> OilTypeIds { get; set; }
        public string OilTypeNames { get; set; }
        public List<long> UserIds { get; set; }
        public string UserNames { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public long LoginUserId { get; set; }
    }

    public class SaudaBookingConfigurationExportDto
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
        public string OilType { get; set; }
        public string UserName { get; set; }
        public string StartDate { get; set; }
        public string IsActive { get; set; }
    }
}
