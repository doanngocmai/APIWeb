using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.News
{
    public class NewsModel
    {
        public int ID { get; set; }
        public int IsBanner { get; set; }
        public string Title { get; set; }
        public int? Type { get; set; }
        public int? TypePost { get; set; }
        public int? Index { get; set; }
        public string UrlImage { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int? Status { get; set; }
    }
    public class NewsHomeModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int? Type { get; set; }
        public int? TypePost { get; set; }
        public string UrlImage { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreateDate { get; set; }
        public int? Status { get; set; }
        public int? Index { get; set; }
    }
}
