using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class SurveyAnswer: BaseModel
    {
        public string Answer { get; set; } 
        public int SurveyQuestionID { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
    }
}
