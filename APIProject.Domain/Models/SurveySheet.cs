using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIProject.Domain.Models
{
    public class SurveySheet : BaseModel
    {
        public string Content { get; set; }
        public int? SurveyAnswerID { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int SurveyQuestionID { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
    }
}
