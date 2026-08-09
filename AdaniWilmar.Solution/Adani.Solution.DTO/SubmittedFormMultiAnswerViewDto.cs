namespace Adani.Solution.DTO
{
    public class SubmittedFormMultiAnswerViewDto
    {
        public long AnswerOptionId { get; set; }
        public string Option { get; set; }
        public bool? IsSelected { get; set; }
    }
}
