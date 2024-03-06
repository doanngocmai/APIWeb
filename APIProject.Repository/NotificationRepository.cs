using APIProject.Common.DTOs.Notification;
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

namespace APIProject.Repository
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IPagedList<ListNotificationModel>> GetListNotification(int page, int limit, int cusID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from n in DbContext.Notifications
                                 where n.IsActive.Equals(SystemParam.ACTIVE)
                                 && n.CustomerID == cusID && (n.Type.Equals(SystemParam.TYPE_NOTIFICATION_NEWS) ? (n.News.IsActive.Equals(SystemParam.ACTIVE) && n.News.Status.Equals(SystemParam.ACTIVE)) : true)
                                 orderby n.ID descending
                                 select new ListNotificationModel
                                 {
                                     ID = n.ID,
                                     CustomerID = n.CustomerID,
                                     NewsID = n.NewsID,
                                     Title = n.Title,
                                     Type = n.Type,
                                     Viewed = n.Viewed,
                                     CreateDate = n.CreatedDate.ToString("hh:mm dd/MM/yyyy"),
                                 }).AsQueryable().ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
