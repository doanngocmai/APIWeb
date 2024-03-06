using APIProject.Common.DTOs.News;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using PagedList.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Utils;
using APIProject.Service.Utils;
using Microsoft.EntityFrameworkCore;
using APIProject.Common.DTOs.Stall;
using APIProject.Common.DTOs.Event;
using APIProject.Common.DTOs.EventParticipant;
using APIProject.Common.DTOs.Gift;

namespace APIProject.Repository
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IPagedList<NewsModel>> GetListNews(int page, int limit, string searchKey, int? type, int? status, string fromDate, string toDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);
                    var model = (from news in DbContext.News
                                 where news.IsActive.Equals(SystemParam.ACTIVE) && news.Type != SystemParam.TYPE_NEWS_EVENT
                                 && (!string.IsNullOrEmpty(searchKey) ? news.Title.Contains(searchKey) : true)
                                 && (type.HasValue ? news.Type.Equals(type) : true)
                                 && (status.HasValue ? news.Status.Equals(status) : true)
                                 && (fd.HasValue ? news.CreatedDate >= fd : true)
                                 && (td.HasValue ? news.CreatedDate <= td : true)
                                 orderby news.CreatedDate descending
                                 select new NewsModel
                                 {
                                     ID = news.ID,
                                     Title = news.Title,
                                     Type = news.Type,
                                     IsBanner = news.IsBanner,
                                     TypePost = news.TypePost,
                                     Status = news.Status,
                                     UrlImage = news.UrlImage,
                                     CreateDate = news.CreatedDate,
                                     Index = news.Index,
                                     StartDate = news.StartDate.HasValue ? news.StartDate.Value.ToString(SystemParam.CONVERT_DATETIME) : "",
                                     EndDate = news.EndDate.HasValue ? news.EndDate.Value.ToString(SystemParam.CONVERT_DATETIME) : "",
                                 }).ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IPagedList<EventModel>> GetListEvent(int page, int limit, string searchKey, int? status, string fromDate, string toDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);
                    var model = (from news in DbContext.News
                                 where news.IsActive.Equals(SystemParam.ACTIVE)
                                 && news.Type.Equals(SystemParam.TYPE_NEWS_EVENT)
                                 && (!string.IsNullOrEmpty(searchKey) ? news.Title.Contains(searchKey) : true)
                                 && (status.HasValue ? news.Status.Equals(status) : true)
                                 && (fd.HasValue ? news.CreatedDate >= fd : true)
                                 && (td.HasValue ? news.CreatedDate <= td : true)
                                 orderby news.CreatedDate descending
                                 select new EventModel
                                 {
                                     Title = news.Title,
                                     ID = news.ID,
                                     TotalGift = news.GiftEvent.Sum(x => x.Quantity),
                                     Status = news.Status,
                                     CreateDate = news.CreatedDate
                                 }).ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IPagedList<NewsHomeModel>> GetListNews(int page, int limit, string searchKey, int? type, string fromDate, string toDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);
                    var model = (from news in DbContext.News
                                 where news.IsActive.Equals(SystemParam.ACTIVE)
                                 && news.Status.Equals(SystemParam.ACTIVE)
                                 && news.TypePost.Equals(SystemParam.NEWS_TYPEPOST_POSTED)
                                 && (!string.IsNullOrEmpty(searchKey) ? news.Title.Contains(searchKey) : true)
                                 && (type.HasValue ? news.Type.Equals(type) : true)
                                 //&& (news.Type.Equals(SystemParam.TYPE_NEWS_EVENT) ? (news.StartDate <= DateTime.Now && news.EndDate >= DateTime.Now) : true)
                                 && (fd.HasValue ? news.CreatedDate >= fd : true)
                                 && (td.HasValue ? news.CreatedDate <= td : true)
                                 select new NewsHomeModel
                                 {
                                     ID = news.ID,
                                     Title = news.Title,
                                     Type = news.Type,
                                     TypePost = news.TypePost,
                                     Status = news.Status,
                                     Index = news.Index,
                                     UrlImage = news.UrlImage,
                                     CreateDate = news.CreatedDate.ToString("dd/MM/yyyy"),
                                     StartDate = news.StartDate.HasValue ? news.StartDate.Value.ToString("dd/MM/yyyy") : "",
                                     EndDate = news.EndDate.HasValue ? news.EndDate.Value.ToString("dd/MM/yyyy") : "",
                                 }).OrderBy(x => x.Index.HasValue ? x.Index.Value : 0).ThenByDescending(x => x.ID).AsQueryable().ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IList<NewsHomeModel>> GetNewsRelated(int ID)
        {
            try
            {
                var model = await (from news in DbContext.News
                                   where news.IsActive.Equals(SystemParam.ACTIVE)
                                   && news.ID != ID
                                   && news.StartDate <= DateTime.Now
                                   && news.EndDate >= DateTime.Now
                                   && news.Status.Equals(SystemParam.ACTIVE)
                                   && news.TypePost.Equals(SystemParam.ACTIVE)
                                   select new NewsHomeModel
                                   {
                                       ID = news.ID,
                                       Title = news.Title,
                                       Type = news.Type,
                                       TypePost = news.TypePost,
                                       Status = news.Status,
                                       UrlImage = news.UrlImage,
                                       CreateDate = news.CreatedDate.ToString("dd/MM/yyyy"),
                                       StartDate = news.StartDate.HasValue ? news.StartDate.Value.ToString("dd/MM/yyyy") : "",
                                       EndDate = news.EndDate.HasValue ? news.EndDate.Value.ToString("dd/MM/yyyy") : "",
                                   }).OrderBy(x => x.Index.HasValue ? x.Index.Value : 0).ThenByDescending(x => x.ID).Take(SystemParam.LIMIT_DEFAULT).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NewsDetailModel> GetNewsDetail(int ID)
        {
            try
            {
                var model = await (from news in DbContext.News
                                   where news.IsActive.Equals(SystemParam.ACTIVE) && news.ID.Equals(ID)
                                   select new NewsDetailModel
                                   {
                                       UserID = news.UserID,
                                       Title = news.Title,
                                       IsBanner = news.IsBanner,
                                       IsPopup = news.IsPopup,
                                       Type = news.Type,
                                       TypePost = news.TypePost,
                                       Content = news.Content,
                                       UrlImage = news.UrlImage,
                                       StartDate = news.StartDate,
                                       EndDate = news.EndDate,
                                       Index = news.Index,
                                       Status = news.Status,
                                       ListRelatedStall = (from relatedstall in DbContext.RelatedStalls
                                                           join s in DbContext.Stalls on relatedstall.StallID equals s.ID
                                                           where relatedstall.NewsID == news.ID && s.IsActive.Equals(SystemParam.ACTIVE)
                                                           select new StallModel
                                                           {
                                                               ID = s.ID,
                                                               Name = s.Name,
                                                               Code = s.Code,
                                                               Floor = s.Floor,
                                                               Phone = s.Phone,
                                                               Logo = s.Logo,
                                                               Status = s.Status,
                                                               CategoryID = s.CategoryID,
                                                               CategoryName = s.Category.Name,
                                                           }).ToList(),
                                       ListGiftNews = (from a in DbContext.GiftNews
                                                       join s in DbContext.Gifts on a.GiftID equals s.ID
                                                       where a.NewsID == news.ID && s.IsActive.Equals(SystemParam.ACTIVE)
                                                       select new ListGiftNews
                                                       {
                                                           ID = s.ID,
                                                           Name = s.Name,

                                                       }).ToList(),
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<NewsDetailAppModel> GetNewsDetailApp(int ID)
        {
            try
            {
                var model = await (from news in DbContext.News
                                   where news.IsActive.Equals(SystemParam.ACTIVE) && news.ID.Equals(ID)
                                   select new NewsDetailAppModel
                                   {
                                       UserID = news.UserID,
                                       Title = news.Title,
                                       IsBanner = news.IsBanner,
                                       IsPopup = news.IsPopup,
                                       Type = news.Type,
                                       TypePost = news.TypePost,
                                       Content = news.Content,
                                       UrlImage = news.UrlImage,
                                       StartDate = news.StartDate,
                                       EndDate = news.EndDate,
                                       Status = news.Status,
                                       ListRelatedNews = (from n in DbContext.News
                                                          where n.Type == news.Type && n.IsActive.Equals(SystemParam.ACTIVE) && n.TypePost.Equals(SystemParam.NEWS_TYPEPOST_POSTED)
                                                          && n.Status.Equals(SystemParam.ACTIVE) && !n.ID.Equals(news.ID)
                                                          //&& (n.Type.Equals(SystemParam.TYPE_NEWS_EVENT) ? (news.StartDate <= DateTime.Now && news.EndDate >= DateTime.Now) : true)
                                                          orderby n.Index
                                                          select new NewsHomeModel
                                                          {

                                                              ID = n.ID,
                                                              Title = n.Title,
                                                              Type = n.Type,
                                                              TypePost = n.TypePost,
                                                              Status = n.Status,
                                                              UrlImage = n.UrlImage,
                                                              CreateDate = n.CreatedDate.ToString("dd/MM/yyyy"),
                                                              StartDate = n.StartDate.HasValue ? n.StartDate.Value.ToString("dd/MM/yyyy") : "",
                                                              EndDate = n.EndDate.HasValue ? n.EndDate.Value.ToString("dd/MM/yyyy") : "",
                                                          }).ToList(),
                                       ListGiftNews = (from a in DbContext.GiftNews
                                                       join s in DbContext.Gifts on a.GiftID equals s.ID
                                                       where a.NewsID == news.ID && s.IsActive.Equals(SystemParam.ACTIVE) && (s.ToDate.HasValue ? s.ToDate.Value > DateTime.Now : true)
                                                       select new ListGiftNews
                                                       {
                                                           ID = s.ID,
                                                           Name = s.Name,
                                                           UrlImage = s.UrlImage,
                                                           Expiry = s.ToDate.HasValue ? s.ToDate.Value.ToString("dd/MM/yyyy") : "",

                                                       }).ToList(),

                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EventDetailModel> GetEventDetail(int ID)
        {
            try
            {
                var model = await (from news in DbContext.News
                                   where news.IsActive.Equals(SystemParam.ACTIVE) && news.ID.Equals(ID)
                                   select new EventDetailModel
                                   {
                                       UserID = news.UserID,
                                       Title = news.Title,
                                       Content = news.Content,
                                       UrlImage = news.UrlImage,
                                       StartDate = news.StartDate,
                                       EndDate = news.EndDate,
                                       ListRelatedStall = news.RelatedStalls.Where(x => x.Stall.IsActive.Equals(SystemParam.ACTIVE)).Select(x => x.StallID).ToList(),
                                       ListEventGift = (from giftEvent in DbContext.GiftEvents
                                                        join gift in DbContext.Gifts on giftEvent.GiftID equals gift.ID
                                                        where giftEvent.NewsID.Equals(news.ID) && giftEvent.IsActive.Equals(SystemParam.ACTIVE)
                                                        select new EventGiftModel
                                                        {
                                                            GiftID = giftEvent.GiftID,
                                                            Quantity = giftEvent.Quantity,
                                                            QuantityExchanged = giftEvent.QuantityExchanged,
                                                            Stock = gift.Number,
                                                            Name = gift.Name,
                                                        }).ToList(),
                                       ID = news.ID,
                                       IsPopup = news.IsPopup,
                                       IsBanner = news.IsBanner,
                                       IsNotify = news.IsNotify,
                                       GiftLimit = news.GiftLimit.HasValue ? news.GiftLimit.Value : 0
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<NewsModel>> GetListNewsBanner()
        {
            try
            {
                var model = await (from news in DbContext.News
                                   where news.IsActive.Equals(SystemParam.ACTIVE) && news.IsBanner.Equals(SystemParam.ACTIVE)
                                   && news.Status.Equals(SystemParam.ACTIVE)
                                   && news.TypePost.Equals(SystemParam.NEWS_TYPEPOST_POSTED)
                                   select new NewsModel
                                   {
                                       ID = news.ID,
                                       Title = news.Title,
                                       Type = news.Type,
                                       TypePost = news.TypePost,
                                       Status = news.Status,
                                       IsBanner = news.IsBanner,
                                       Index = news.Index,
                                       CreateDate = news.CreatedDate,
                                       UrlImage = news.UrlImage,
                                       StartDate = news.StartDate.HasValue ? news.StartDate.Value.ToString(SystemParam.CONVERT_DATETIME) : "",
                                       EndDate = news.EndDate.HasValue ? news.EndDate.Value.ToString(SystemParam.CONVERT_DATETIME) : "",
                                   }).OrderBy(x => x.Index.HasValue ? x.Index.Value : 0).ThenByDescending(x => x.ID).Take(SystemParam.LIMIT_BANNER).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EventListModel>> GetListEvent()
        {
            try
            {
                var model = await (from news in DbContext.News
                                   where news.IsActive.Equals(SystemParam.ACTIVE) && news.Type.Equals(SystemParam.TYPE_NEWS_EVENT)
                                   && news.Status.Equals(SystemParam.ACTIVE) && DateTime.Now > news.StartDate && DateTime.Now < news.EndDate
                                   orderby news.CreatedDate descending
                                   select new EventListModel
                                   {
                                       ID = news.ID,
                                       Name = news.Title,
                                       GiftLimit = news.GiftLimit
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<EventModel>> GetListEventStatistic()
        {
            try
            {
                var model = await (from news in DbContext.News
                                   where news.IsActive.Equals(SystemParam.ACTIVE) && news.Type.Equals(SystemParam.TYPE_NEWS_EVENT)
                                   && news.Status.Equals(SystemParam.ACTIVE) 
                                   orderby news.CreatedDate descending
                                   select new EventModel
                                   {
                                       Title = news.Title,
                                       ID = news.ID,
                                       Status = news.Status,
                                       CreateDate = news.CreatedDate
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<NewsHomeModel> GetNewsPopup()
        {
            try
            {
                var model = await (from news in DbContext.News
                                   where news.IsActive.Equals(SystemParam.ACTIVE) && news.IsPopup.Equals(SystemParam.ACTIVE)
                                   && news.Status.Equals(SystemParam.ACTIVE)
                                   && (news.StartDate.HasValue ? news.StartDate.Value < DateTime.Now : true)
                                   && (news.EndDate.HasValue ? news.EndDate.Value > DateTime.Now : true)
                                   orderby news.ID descending
                                   select new NewsHomeModel
                                   {
                                       ID = news.ID,
                                       UrlImage = news.UrlImage,
                                       StartDate = news.StartDate.HasValue ? news.StartDate.Value.ToString("dd/MM/yyyy") : "",
                                       EndDate = news.EndDate.HasValue ? news.EndDate.Value.ToString("dd/MM/yyyy") : "",
                                       Status = news.Status,
                                       CreateDate = news.CreatedDate.ToString("dd/MM/yyyy"),
                                       Title = news.Title,
                                       Type = news.Type,
                                       TypePost = news.TypePost
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
