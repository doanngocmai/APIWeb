using APIProject.Common.DTOs.Bill;
using APIProject.Common.DTOs.Stall;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IStallRepository : IRepository<Stall>
    {
        Task<IList<SumBillStallModel>> GetSumBillStall(int TypeOrder,int? EventID);
        Task<IPagedList<StallModel>> GetListStall(int page, int limit, int? floor, int? status, string searchKey, int? categoryID);
        Task<List<StallModel>> GetListStall(int page, int limit, int? floor, string searchKey, int? categoryID,int? orderBy);
        Task<List<StallModel>> GetListStall();
        Task<IList<StallModel>> GetStallRelated(int ID);
        Task<StallDetailModel> GetStallDetail(int ID);
        Task<IList<SumBillStallModel>> GetAllSumBillStall();
    }
}
