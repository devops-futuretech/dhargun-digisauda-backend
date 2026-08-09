using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class ChatBotSpecialRateApprovalDto
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string SkuName { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal SpecialPrice { get; set; }
        public string FreightRoute { get; set; }
        public string IncoTerms { get; set; }
    }
}
