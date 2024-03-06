using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Category
{
    public class CreateCategoryModel
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }
    }
}
