using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class FormInputDto : LoginUserIdDto
    {
        public long Id { get; set; }
        public long FormId { get; set; }
        public string FormName { get; set; }
        public string UserRoleType { get; set; }
        public long UserId { get; set; }
        public string CustomerName { get; set; }
        public int SubmittedFormId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<QuestionAnswerInput> QuestionAnswer { get; set; }

    }
    public class QuestionAnswerInput
    {
        public long QuestionId { get; set; }
        public string Query { get; set; }
        public string QuestionTypeName { get; set; }
        public string Answer { get; set; }
    }
}
