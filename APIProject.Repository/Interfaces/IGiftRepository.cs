using APIProject.Common.DTOs.Gift;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IGiftRepository:IRepository<Gift>
    {
        Task<IPagedList<ListGiftModel>> GetListGift(int page, int limit, string searchKey, int? type, string fromDate, string toDate);
        Task<List<ListGiftNews>> GetListGift();
        Task<GiftDetailModel> GetGiftDetail(int ID);
        Task<IPagedList<ListChangeGiftModel>> GetListExchangeGift(int page, int limit, string searchKey, int? type, string fromDate, string toDate);
        Task<IPagedList<GiftCodeModel>> GetListGiftCode(int page, int limit, string searchKey, int? status, int GiftID);
        
        
        //API APP
        Task<IPagedList<VoucherModel>> GetListVoucher(int page, int limit);
        Task<IPagedList<MyVoucherModel>> GetMyListVoucher(int page, int limit, int status,int CustomerID);
        Task<IPagedList<MyVoucherModel>> GetMyListVoucherExpired(int page, int limit,int CustomerID);
        Task<MyVoucherModel> GetMyVoucherDetail(int ID);
        Task<VoucherModel> GetVoucherDetail(int ID);
    }
}
