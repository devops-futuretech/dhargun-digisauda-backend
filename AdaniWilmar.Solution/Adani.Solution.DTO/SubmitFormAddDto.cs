using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SubmitFormAddDto : LoginUserIdDto
    {
        public long SubmittedFormId { get; set; }
        public long FormId { get; set; }        
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Remarks { get; set; }
        public long? ParentFormId { get; set; }
        public long? DemoUserId { get; set; }
        public long? DemoId { get; set; }
        public bool IsStatusResolved { get; set; }
        public long SkuId { get; set; }
        public long PlantId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public long DealerId { get; set; }
        public string DealerName { get; set; }
        public IList<SubmitFormQuestionAddDto> Questions { get; set; }

        public SubmitFormAddDto()
        {
            Questions = new List<SubmitFormQuestionAddDto>();
        }
    }
}
