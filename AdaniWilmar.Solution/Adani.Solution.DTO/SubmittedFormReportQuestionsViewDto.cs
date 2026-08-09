namespace Adani.Solution.DTO
{
    public class SubmittedFormReportQuestionsViewDto
    {
        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public long QuestionId { get; set; }
        public string Question { get; set; }
        public long QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public string Answer { get; set; }
    }
}
