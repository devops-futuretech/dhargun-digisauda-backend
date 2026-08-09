using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FinancialYearDto
    {
        public long Id { get; set; }
        public string EncryptedId { get; set; }
        public string Year { get; set; }
        public DateTime EffectiveFrom { get; set; } = DateTime.Now;
        public DateTime EffectiveTo { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public long LoginUserId { get; set; }
        public string EffectiveFromstring { get; set; }
        public string EffectiveTostring { get; set; }
    }
}
