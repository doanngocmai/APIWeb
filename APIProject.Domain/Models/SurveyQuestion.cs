using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class SurveyQuestion:BaseModel
    {
        public string Question { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public ICollection<SurveyAnswer> SurveyAnswers { get; set; }
    }
}
