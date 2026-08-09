using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SaudaConversionDetailInputDto : UserIdDto
    {
        public long SaudaConversionId { get; set; }
        public bool isConversion { get; set; }
    }

    public class SaudaConversionUpdateDto : IAPIInputDTO
    {
        public SaudaConversionUpdateDto()
        {
            SaudaIds = new List<long>();
        }

        public long SaudaId { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public long ModifiedBy { get; set; }
        public List<long> SaudaIds { get; set; }

        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }

    public class SaudaConversionApprovalInputDto:LoginUserIdDto
    {
        public long Id { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
    }
}
