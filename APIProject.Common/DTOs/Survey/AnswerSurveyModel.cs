using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Survey
{
    public class AnswerSurveyModel
    {
        public int QuestionID { get; set; }
        public int? AnswerID { get; set; }
        public string Content { get; set; }
    }
}
