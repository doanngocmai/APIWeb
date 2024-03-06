using APIProject.Common.DTOs.Category;
using APIProject.Common.DTOs.EventParticipant;
using APIProject.Common.DTOs.News;
using APIProject.Common.DTOs.Stall;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Common.DTOs.Home
{
    public class HomeModel
    {
        public List<NewsModel> ListBanner { get; set; }
        public IPagedList<NewsHomeModel> ListEvent { get; set; }
        public IPagedList<NewsHomeModel> ListPromotion { get; set; }
        public IPagedList<NewsHomeModel> ListCampaign { get; set; }
        public IPagedList<CategoryModel> ListCategory { get; set; }
        public NewsHomeModel NewsPopup { get; set; }
    }
    public class HomeStaffModel
    {
        public List<NewsModel> ListBanner { get; set; }
        public IPagedList<NewsHomeModel> ListEvent { get; set; }
    }
}
