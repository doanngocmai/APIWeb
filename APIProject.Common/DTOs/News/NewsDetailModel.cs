using APIProject.Common.DTOs.Gift;
using APIProject.Common.DTOs.Stall;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.News
{
    public class NewsDetailModel
    {
        public int UserID { get; set; }
        public int IsBanner { get; set; }
        public int IsPopup { get; set; }
        public int? Index { get; set; }
        public string Title { get; set; }//Tiêu đề
        public int? Type { get; set; }//kiểu tin tức
        public int? TypePost { get; set; }//kiểu đăng bài-lưu nháp
        public string Content { get; set; }
        public string UrlImage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }
        public IList<StallModel> ListRelatedStall { get; set; }
        public IList<ListGiftNews> ListGiftNews { get; set; }
    }
    public class NewsDetailAppModel
    {
        public int UserID { get; set; }
        public int IsBanner { get; set; }
        public int IsPopup { get; set; }
        public string Title { get; set; }//Tiêu đề
        public int? Type { get; set; }//kiểu tin tức
        public int? TypePost { get; set; }//kiểu đăng bài-lưu nháp
        public string Content { get; set; }
        public string UrlImage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }
        public IList<NewsHomeModel> ListRelatedNews { get; set; }
        public IList<ListGiftNews> ListGiftNews { get; set; }
    }
}
