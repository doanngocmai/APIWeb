using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.News
{
    public class CreateNewsModel
    {
        public int UserID { get; set; }
        public string Title { get; set; }//Tiêu đề
        public int Type { get; set; }//kiểu tin tức
        public int TypePost { get; set; }//kiểu đăng bài-lưu nháp
        public int IsBanner { get; set; }
        public int IsPopup { get; set; }
        public int IsNotify { get; set; }
        public int? Index { get; set; }
        public string Content { get; set; }
        public string UrlImage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> ListRelatedStall { get; set; }
        public List<int> ListGiftNews { get; set; }
    }

    public class UpdateNewsModel : CreateNewsModel
    {
        public int ID { get; set; }
    }
    public class RelatedStallModel
    {
        public int ID { get; set; }
        public int StallID { get; set; }
        public int NewsID { get; set; }
    }
}
