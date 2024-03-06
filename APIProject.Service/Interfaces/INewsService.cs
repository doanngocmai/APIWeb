using APIProject.Common.DTOs.News;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface INewsService
    {
        Task<JsonResultModel> GetListNews(int page, int limit, string searchKey, int? type, int? status, string fromDate, string toDate);
        Task<JsonResultModel> GetNewsDetail(int ID);
        Task<JsonResultModel> GetNewsDetailApp(int ID);
        Task<JsonResultModel> CreateNews(CreateNewsModel input);
        Task<JsonResultModel> UpdateNews(UpdateNewsModel input);
        Task<JsonResultModel> DeleteNews(int ID);
        Task<JsonResultModel> GetListNews(int page, int limit, string searchKey, int? type, string fromDate, string toDate);
        Task<JsonResultModel> GetListNewsBanner();
        Task<JsonResultModel> GetNewsRelated(int ID);
        Task<JsonResultModel> ChangeStatus(int ID);
    }
}
