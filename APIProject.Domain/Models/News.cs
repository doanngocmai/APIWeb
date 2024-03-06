using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Domain.Models
{
    public class News : BaseModel
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public string Title { get; set; }//Tiêu đề
        public int Type { get; set; }//kiểu tin tức
        public int TypePost { get; set; }//kiểu đăng bài-lưu nháp
        public string Content { get; set; }
        public string UrlImage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public int? Index { get; set; }
        public int IsBanner { get; set; }
        public int IsPopup { get; set; }
        public int IsNotify { get; set; }
        public int? GiftLimit { get; set; }
        public ICollection<GiftEvent> GiftEvent { get; set; }
        public ICollection<Gift> Gifts { get; set; }
        public ICollection<RelatedStall> RelatedStalls { get; set; }
        public ICollection<EventParticipant> EventParticipants { get; set; }
    }
}
