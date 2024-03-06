using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Stall
{
    public class CreateStallModel
    {
        public string Name { get; set; }
        public int Floor { get; set; }
        public int? Index { get; set; }
        public string Phone { get; set; }
        public string LinkWeb { get; set; }
        public string LinkFB { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public List<int> ListPromotionID { get; set; }
    }
}
