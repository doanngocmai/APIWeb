using APIProject.Common.DTOs.Gift;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IGiftService
    {
        Task<JsonResultModel> GetListGift(int page, int limit, string searchKey, int? type, string fromDate, string toDate);
        Task<JsonResultModel> GetListGift();
        Task<JsonResultModel> GetGiftDetail(int ID);
        Task<JsonResultModel> GetListExchangeGift(int page, int limit, string searchKey, int? type, string fromDate, string toDate);
        Task<JsonResultModel> GetListGiftCode(int page, int limit, string searchKey, int? status, int GiftID);
        Task<JsonResultModel> CreateGift(CreateGiftModel input);
        Task<JsonResultModel> UpdateGift(UpdateGiftModel input);
        Task<JsonResultModel> DeleteGift(int ID);
        Task<JsonResultModel> ChangeStatus(int ID);
        Task<JsonResultModel> CreateGiftCode(CreateGiftCodeModel input);
        Task<JsonResultModel> UpdateGiftCode(UpdateGiftCodeModel input);
        Task<JsonResultModel> DeleteGiftCode(int ID);
        Task<JsonResultModel> ImportVoucherCode(HttpContext context);
        Task<JsonResultModel> ImportVoucherCode(int GiftID, HttpContext context, IWebHostEnvironment webHostEnvironment);

        //API APP

        Task<JsonResultModel> GetListVoucher(int page, int limit);
        Task<JsonResultModel> GetMyListVoucher(int page, int limit, int status, int CustomerID);
        Task<JsonResultModel> GetMyListVoucherExpired(int page, int limit, int CustomerID);
        Task<JsonResultModel> GetMyVoucherDetail(int ID);
        Task<JsonResultModel> GetVoucherDetail(int ID);
        Task<JsonResultModel> ExchangeVoucher(int ID,int CustomerID);
        Task<JsonResultModel> UseVoucher(int ID);
    }
}
