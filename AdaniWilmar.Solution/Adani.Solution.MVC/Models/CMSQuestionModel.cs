using Adani.Solution.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.Solution.MVC.Models
{
    public class CMSQuestionModel : QuestionAddDto, IAPIInputDTO
    {
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
        public bool IsEdit { get; set; }
    }
}