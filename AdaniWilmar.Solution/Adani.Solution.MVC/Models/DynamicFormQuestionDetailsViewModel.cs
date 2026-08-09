using Adani.Solution.DTO;
using System.Collections.Generic;

namespace Adani.Solution.MVC.Models
{
    public class DynamicFormQuestionDetailsViewModel : FormQuestionsViewDto
    {
        public long LoginUserId { get; set; }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public bool IsEdit { get; set; }
        public IList<FormQuestionAddDto> RemovedQuestions { get; set; }
        public IList<FormQuestionAddDto> AddedQuestions { get; set; }
        public string SavedQuestionsString { get; set; }
        public bool CreateAnother { get; set; }
        public bool OnSectionChangeEdit { get; set; }
        public string SelectedQuestionsString { get; set; }
        public string UnselectedQuestionsString { get; set; }
        public string FormUserString { get; set; }
        public string QuestionTypeName { get; set; }
        public string Query { get; set; }
        public long SectionId { get; set; }
        public long FormId { get; set; }
        public List<SectionQuestionsViewDto> SectionQuestionsList { get; set; }
        public List<long> RoleIds { get; set; }
        public DynamicFormQuestionDetailsViewModel()
        {
            RemovedQuestions = new List<FormQuestionAddDto>();
            AddedQuestions = new List<FormQuestionAddDto>();
            SectionQuestionsList = new List<SectionQuestionsViewDto>();
            RoleIds = new List<long>();
        }
    }
}