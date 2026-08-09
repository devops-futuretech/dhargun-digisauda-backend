using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FormUpdateDto : LoginUserIdDto
    {
        public long FormId { get; set; }        
        public string FormName { get; set; }
        public long? ParentFormId { get; set; }
        public bool IsActive { get; set; }
        public bool IsFormStatus { get; set; }
        public List<long> RoleIds { get; set; }
        public IList<long> NewUsers { get; set; }
        public IList<SectionQuestionsViewDto> SectionQuestions { get; set; }
        public IList<FormQuestionAddDto> NewQuestions { get; set; }
        public IList<FormQuestionAddDto> RemovedQuestions { get; set; }
        //public List<SectionQuestionsViewDto> SectionQuestionsList { get; set; }
        public FormUpdateDto()
        {
            NewQuestions = new List<FormQuestionAddDto>();
            RemovedQuestions = new List<FormQuestionAddDto>();
            SectionQuestions = new List<SectionQuestionsViewDto>();
            //SectionQuestionsList = new List<SectionQuestionsViewDto>();
            NewUsers = new List<long>();
            RoleIds = new List<long>();

        }
}
}
