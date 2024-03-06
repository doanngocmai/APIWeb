using APIProject.Common.DTOs.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Stall
{
    public class StallDetailModel : StallModel
    {
        public string LinkWeb { get; set; }
        public string LinkFB { get; set; }
        public string Description { get; set; }
        public string CategoryImage { get; set; }
        public List<NewsModel> ListPromotion { get; set; }
        public List<OtherStall> ListOtherStall { get; set; }
    }
    public class OtherStall
    {
        public int ID { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int Floor { get; set; }
    }

}
