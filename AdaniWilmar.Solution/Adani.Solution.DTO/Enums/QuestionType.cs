using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO.Enums
{
    public enum QuestionType
    {
        [Description("Text Entry")] TextEntry = 1,
        [Description("Yes or No")] YesOrNo = 2, //6
        [Description("Single Choice")] SingleChoice = 3, //5     
        [Description("Multiple Choice")] MultipleChoice = 4,
        //[Description("Attachments")] Attachments = 5
    }
}
