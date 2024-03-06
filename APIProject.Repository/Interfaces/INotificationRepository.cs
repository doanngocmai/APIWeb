using APIProject.Common.DTOs.Notification;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface INotificationRepository:IRepository<Notification>
    {
        Task<IPagedList<ListNotificationModel>> GetListNotification(int page, int limit, int cusID);
    }
}
