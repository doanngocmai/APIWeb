using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Survey
{
    public class AnswerStatisticModel
    {
        public int ID { get; set; }
        public string Answer { get; set; }
        public int Count { get; set; }
    }
    public class ContentStatisticModel
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string Content { get; set; }
    }
}
