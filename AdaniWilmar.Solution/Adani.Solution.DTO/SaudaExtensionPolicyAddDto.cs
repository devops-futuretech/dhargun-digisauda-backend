using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SaudaExtensionPolicyAddDto :UserIdDto
    {
        public List<long> ZonalHeadIds { get; set; }
        public List<long> OilIds { get; set; }
        public List<int> StateIds { get; set; }
        public long Days { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long VerticalId { get; set; }
    }

    public class SaudaExtensionPolicyViewDto
    {
        public long Id { get; set; }
        public long OilId { get; set; }
        public string OilTypeName { get; set; }
        public string OilTypeCode { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public long Days { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class SaudaExtensionPolicyExportDto
    {
        public string OilType { get; set; }
        public string State { get; set; }
        public long Days { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public bool IsActive { get; set; }
      }
}
