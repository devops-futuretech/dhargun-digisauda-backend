using Adani.Solution.DTO;

namespace Adani.Solution.MVC.Models
{
    public class HeadQuartersViewModel : HeadquartersDto
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long CreatedBy { get; set; }
    }
}