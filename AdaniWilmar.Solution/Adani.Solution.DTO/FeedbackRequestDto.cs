using System;

namespace Adani.Solution.DTO
{
    public class FeedbackRequestDto : CommonResultDto
    {
        public long FeedbackId { get; set; }
        public long FeedbackTypeId { get; set; }
        public string Details { get; set; }
        public string FeedbackType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}
