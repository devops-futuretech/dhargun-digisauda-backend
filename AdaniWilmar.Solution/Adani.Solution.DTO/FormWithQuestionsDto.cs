using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class SubmittedForms
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public DateTime SubmissionDate { get; set; }
    }

    public class SubmittedFormQuestions
    {
        public int Id { get; set; }
        public int SubmittedFormId { get; set; }
        public string QuestionText { get; set; }
        public string Query { get; set; }
        public string QuestionTypeName { get; set; }
        public string Answer { get; set; }
    }
    public class SubmittedFormDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public long FormId { get; set; }
        public string FormName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}

