using Adani.Solution.DTO;

namespace Adani.Solution.MVC.Models
{
    public class ReasonsViewModel : ReasonDto
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long CreatedBy { get; set; }
    }
}