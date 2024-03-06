using APIProject.Domain.Models;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface INotificationService
    {
        Task<JsonResultModel> GetListNotification(int page, int limit, int cusID);
        Task<JsonResultModel> ReadAllNotification(int cusID);
        Task<JsonResultModel> ReadNotification(int ID);
        Task CreateNotification(Customer cus, string content, int type, int? newsID);
        Task CreateNotification(IList<Customer> listCus, string content, int type, int? newsID);
    }
}
