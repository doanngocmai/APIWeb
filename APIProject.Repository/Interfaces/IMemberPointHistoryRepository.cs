using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.DTOs.Bill;
using APIProject.Common.DTOs.MenberPointHistory;
using APIProject.Domain.Models;
using PagedList.Core;

namespace APIProject.Repository.Interfaces
{
    public interface IMemberPointHistoryRepository:IRepository<MemberPointHistory>
    {
        Task<IPagedList<MemberPointHistoryModel>> GetListPointHistory(int page, int limit, int? type, int? customerID, string EvenName, string fromDate, string toDate);
        Task<IPagedList<ListBillModel>> GetPointHistoryDetail(int page, int limit, int ID);
        Task<IPagedList<MemberChangeHistoryModel>> GetListChangeHistory(int page, int limit, int? type, int? customerID, string searchKey, string fromDate, string toDate);
        Task<IPagedList<PointHistoryModel>> GetListPointHistoryModel(int page, int limit, int? type, int customerID);
    }
}
