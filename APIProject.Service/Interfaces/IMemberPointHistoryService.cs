using APIProject.Service.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IMemberPointHistoryService
    {
        Task<JsonResultModel> GetListPointHistory(int page, int limit, int? type, int? customerID, string EvenName, string fromDate, string toDate);
        Task<JsonResultModel> GetPointHistoryDetail(int page, int limit, int ID);
        Task<JsonResultModel> GetListChangeHistory(int page, int limit, int? type, int? customerID, string searchKey, string fromDate, string toDate);
        Task<JsonResultModel> GetPointHistoryApp(int page, int limit, int? type, int customerID);
    }
}
