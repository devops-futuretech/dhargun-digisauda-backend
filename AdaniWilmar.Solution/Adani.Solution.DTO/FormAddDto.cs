using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adani.Solution.DTO
{
    public class FormAddDto : LoginUserIdDto
    {
        public long FormId { get; set; }        
        public IList<SectionQuestionsViewDto> SectionQuestions { get; set; }
        public IList<long> FormUsers { get; set; }
        public string FormName { get; set; }
        public long? ParentFormId { get; set; }
        public bool IsActive { get; set; }
        public bool IsFormStatus { get; set; }
        public List<long> RoleIds { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedDate { get; set; }
        public FormAddDto()
        {
            SectionQuestions = new List<SectionQuestionsViewDto>();
            FormUsers = new List<long>();
            RoleIds = new List<long>();
        }
    }

    public class FormQuestionAddDto
    {
        public long SectionId { get; set; }
        public long QuestionId { get; set; }
        public int OrderNo { get; set; }
    }
}
