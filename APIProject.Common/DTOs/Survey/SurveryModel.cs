using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Survey
{
    public class SurveryModel
    {      
        public int ID { get; set; }
        public string Question { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public List<SurveryAnswerModel> ListAnswer { get; set; }
    }
    public class SurveryAnswerModel
    {
        public int ID { get; set; }
        public string Answer { get; set; }
        public string Another { get; set; }
    }
}
