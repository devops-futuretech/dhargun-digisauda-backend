using Adani.Solution.DTO;

namespace Adani.Solution.MVC.Models
{
    public class SpecialRateViewModel: IAPIInputDTO
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public long SpecialRateId { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public long LoginUserId { get; set; }
    }
}