using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SAPEmailStatementInputDto
    {
        public long Id { get; set; }
        public long LoginUserId { get; set; }
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Currency { get; set; }
        public bool IsWithoutSpecialGL { get; set; }
        public int DocumentType { get; set; }
        public bool IsActive { get; set; }
    }

    public class SAPEmailStatementDStatusDto
    {
        public long AccountStatementId { get; set; }
        public string StatusMessage { get; set; }
    }
}
