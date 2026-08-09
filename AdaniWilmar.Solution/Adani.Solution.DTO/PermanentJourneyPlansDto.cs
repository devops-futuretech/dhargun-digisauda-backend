using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class PermanentJourneyPlansDto
    {
        public long PJPId { get; set; }
        public string EncryptedId { get; set; }
        public string PJPNumber { get; set; }
        public long FinancialYearId { get; set; }
        public string FinancialYear { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedUser { get; set; }
        public long StatusId { get; set; }
        public string Status { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
