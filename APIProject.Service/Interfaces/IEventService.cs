using APIProject.Common.DTOs.Event;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IEventService
    {
        Task<JsonResultModel> GetListEvent(int page, int limit, string searchKey, int? status, string fromDate, string toDate);
        Task<JsonResultModel> GetListEventStatistic();
        Task<JsonResultModel> GetEventDetail(int ID);
        Task<JsonResultModel> CreateEvent(CreateEventModel input);
        Task<JsonResultModel> UpdateEvent(UpdateEventModel input);
        Task<JsonResultModel> DeleteEvent(int ID);
        Task<JsonResultModel> ChangeStatus(int ID);

    }
}
