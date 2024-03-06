using APIProject.Common.DTOs.Event;
using APIProject.Common.DTOs.EventParticipant;
using APIProject.Common.DTOs.News;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface INewsRepository : IRepository<News>
    {
        Task<IPagedList<NewsModel>> GetListNews(int page, int limit, string searchKey, int? type, int? status, string fromDate, string toDate);
        Task<IPagedList<EventModel>> GetListEvent(int page, int limit, string searchKey,int? status, string fromDate, string toDate);
        Task<List<EventListModel>> GetListEvent();
        Task<List<EventModel>> GetListEventStatistic();
        Task<NewsDetailModel> GetNewsDetail(int id);
        Task<NewsDetailAppModel> GetNewsDetailApp(int id);
        Task<EventDetailModel> GetEventDetail(int id);
        Task<IPagedList<NewsHomeModel>> GetListNews(int page, int limit, string searchKey, int? type, string fromDate, string toDate);
        Task<List<NewsModel>> GetListNewsBanner();
        Task<IList<NewsHomeModel>> GetNewsRelated(int ID);
        Task<NewsHomeModel> GetNewsPopup();
    }
}
