using Adani.Solution.DTO;

namespace Adani.Solution.MVC.Models
{
    public class FinancialYearViewModel : FinancialYearDto
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}