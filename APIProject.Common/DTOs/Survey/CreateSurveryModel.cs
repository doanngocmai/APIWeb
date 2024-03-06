using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Survey
{
    public class CreateSurveryModel
    {
        public string Question { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public List<string> ListAnswer { get; set; }
    }
    public class UpdateSurveryModel : CreateSurveryModel
    {
        public int ID { get; set; }
    }
}
