using APIProject.Common.DTOs.Gift;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Utils;
using PagedList.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Utils;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class GiftRepository : BaseRepository<Gift>, IGiftRepository
    {
        public GiftRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IPagedList<ListGiftModel>> GetListGift(int page, int limit, string searchKey, int? type, string fromDate, string toDate)
        {
            try
            {
                var fDate = Util.ConvertFromDate(fromDate);
                var tDate = Util.ConvertToDate(toDate);
                return await Task.Run(() =>
                {
                    var model = (from g in DbContext.Gifts
                                 where g.IsActive.Equals(SystemParam.ACTIVE)
                                 && (type.HasValue ? g.Type.Equals(type) : true)
                                 && (!String.IsNullOrEmpty(searchKey) ? g.Name.Contains(searchKey) : true)
                                 && (fDate.HasValue ? g.CreatedDate >= fDate : true)
                                 && (tDate.HasValue ? g.CreatedDate <= tDate : true)
                                 orderby g.CreatedDate descending
                                 select new ListGiftModel
                                 {
                                     ID = g.ID,
                                     Name = g.Name,
                                     Point = g.Point,
                                     Type = g.Type,
                                     Quantity = g.Type == SystemParam.TYPE_GIFT_PRESENTS ? g.Number : g.GiftCodeQRs.Count(x => x.Status.Equals(SystemParam.STATUS_GIFTCODE_NOT_EXCHANGE)),
                                     FromDate = g.FromDate,
                                     ToDate = g.ToDate,
                                     CreateDate = g.CreatedDate,
                                     Status = g.Status,
                                     Description = g.Description,
                                     UrlImage = g.UrlImage
                                 }).ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GiftDetailModel> GetGiftDetail(int ID)
        {
            try
            {
                var model = await (from g in DbContext.Gifts
                                   where g.IsActive.Equals(SystemParam.ACTIVE)
                                   && g.ID.Equals(ID)
                                   orderby g.CreatedDate descending
                                   select new GiftDetailModel
                                   {
                                       Name = g.Name,
                                       Point = g.Point,
                                       Type = g.Type,
                                       Quantity = g.Type == SystemParam.TYPE_GIFT_PRESENTS ? g.Number : g.GiftCodeQRs.Count(x => x.Status.Equals(SystemParam.STATUS_GIFTCODE_NOT_EXCHANGE)),
                                       QuantityUsed = g.GiftCodeQRs.Count(x => !x.Status.Equals(SystemParam.STATUS_GIFTCODE_NOT_EXCHANGE)),
                                       FromDate = g.FromDate,
                                       ToDate = g.ToDate,
                                       CreateDate = g.CreatedDate,
                                       Status = g.Status,
                                       Description = g.Description,
                                       UrlImage = g.UrlImage,
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IPagedList<ListChangeGiftModel>> GetListExchangeGift(int page, int limit, string searchKey, int? type, string fromDate, string toDate)
        {
            try
            {
                var FromDate = Util.ConvertFromDate(fromDate);
                var ToDate = Util.ConvertToDate(toDate);
                return await Task.Run(() =>
                {
                    var model = (from g in DbContext.PointHistories
                                 where g.IsActive.Equals(SystemParam.ACTIVE) && g.Type.Equals(SystemParam.TYPE_MEMBER_HISTORY_EXCHANGED_POINT)
                                 && (!String.IsNullOrEmpty(searchKey) ? (g.Gift.Name.Contains(searchKey) || g.Customer.Name.Contains(searchKey)) : true)
                                 && (FromDate.HasValue ? g.CreatedDate >= FromDate : true)
                                 && (ToDate.HasValue ? g.CreatedDate <= ToDate : true)
                                 && (type.HasValue ? g.Type.Equals(type) : true)
                                 orderby g.CreatedDate descending
                                 select new ListChangeGiftModel
                                 {
                                     ID = g.ID,
                                     GiftID = g.GiftID,
                                     GiftName = g.Gift.Name,
                                     CustomerID = g.CustomerID,
                                     CustomerName = g.Customer.Name,
                                     Type = g.Type,
                                     Point = g.Point,
                                     CreateDate = g.CreatedDate,
                                 }).ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IPagedList<GiftCodeModel>> GetListGiftCode(int page, int limit, string searchKey, int? status, int GiftID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from code in DbContext.GiftCodeQRs
                                 where code.IsActive.Equals(SystemParam.ACTIVE)
                                 && code.GiftID.Equals(GiftID)
                                 && (!String.IsNullOrEmpty(searchKey) ? code.Code.Contains(searchKey) : true)
                                 && (status.HasValue ? code.Status.Equals(status) : true)
                                 orderby code.ID descending
                                 select new GiftCodeModel
                                 {
                                     Code = code.Code,
                                     ID = code.ID,
                                     Status = code.Status,
                                 }).ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IPagedList<VoucherModel>> GetListVoucher(int page, int limit)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from x in DbContext.Gifts
                                 where x.IsActive == SystemParam.ACTIVE && x.Type == SystemParam.TYPE_GIFT_VOURCHER && DateTime.Now < x.ToDate
                                 && x.Status == SystemParam.ACTIVE && x.GiftCodeQRs.Where(g => g.Status.Equals(SystemParam.STATUS_GIFTCODE_NOT_EXCHANGE)).Count() > 0
                                 select new VoucherModel
                                 {
                                     ID = x.ID,
                                     Name = x.Name,
                                     Point = x.Point,
                                     UrlImage = x.UrlImage,
                                     EndDate = x.ToDate.HasValue ? x.ToDate.Value.ToString("dd/MM/yyyy") : "",
                                     FromDate = x.FromDate.HasValue ? x.FromDate.Value.ToString("dd/MM/yyyy") : "",
                                     Description = x.Description,
                                 }).ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        // chi tiết quà tặng của tôi 
        public async Task<MyVoucherModel> GetMyVoucherDetail(int GiftCodeID)
        {
            try
            {
                var model = await (from a in DbContext.Gifts
                                   join b in DbContext.GiftCodeQRs on a.ID equals b.GiftID
                                   where b.ID.Equals(GiftCodeID)
                                   && a.IsActive.Equals(SystemParam.ACTIVE)
                                   select new MyVoucherModel
                                   {
                                       ID = b.ID,
                                       Name = a.Name,
                                       Point = a.Point,
                                       EndDate = a.ToDate.HasValue ? a.ToDate.Value.ToString("dd/MM/yyyy") : "",
                                       FromDate = a.FromDate.HasValue ? a.FromDate.Value.ToString("dd/MM/yyyy") : "",
                                       Description = a.Description,
                                       UrlImage = a.UrlImage,
                                       Code = b.Code,
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //Danh sách quà tặng của tôi
        public async Task<IPagedList<MyVoucherModel>> GetMyListVoucher(int page, int limit, int status, int CustomerID)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var model = (from x in DbContext.Gifts
                                 join a in DbContext.GiftCodeQRs on x.ID equals a.GiftID
                                 where x.IsActive == SystemParam.ACTIVE
                                 && a.CustomerID == CustomerID
                                 && (status == SystemParam.STATUS_GIFTCODE_EXCHANGE ? DateTime.Now < x.ToDate : true)
                                 && a.Status.Equals(status)
                                 orderby a.CreatedDate descending
                                 select new MyVoucherModel
                                 {
                                     ID = a.ID,
                                     Name = x.Name,
                                     UrlImage = x.UrlImage,
                                     EndDate = x.ToDate.HasValue ? x.ToDate.Value.ToString("dd/MM/yyyy") : "",
                                     FromDate = x.FromDate.HasValue ? x.FromDate.Value.ToString("dd/MM/yyyy") : "",
                                     Description = x.Description,
                                     Code = a.Code,
                                     Point = x.Point
                                 }).ToPagedList(page, limit);
                    return model;
                }
                catch (Exception)
                {

                    throw;
                }

            });
        }
        //danh sách quà tặng của tôi hết hạn sử dụng 
        public async Task<IPagedList<MyVoucherModel>> GetMyListVoucherExpired(int page, int limit, int CustomerID)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var model = (from x in DbContext.Gifts
                                 join a in DbContext.GiftCodeQRs on x.ID equals a.GiftID
                                 where x.IsActive.Equals(SystemParam.ACTIVE)
                                 && a.CustomerID.Equals(CustomerID)
                                 && DateTime.Now > x.ToDate && a.Status.Equals(SystemParam.STATUS_GIFTCODE_EXCHANGE)
                                 orderby a.CreatedDate descending
                                 select new MyVoucherModel
                                 {
                                     ID = a.ID,
                                     Name = x.Name,
                                     UrlImage = x.UrlImage,
                                     Code = a.Code,
                                     Point = x.Point,
                                     EndDate = x.ToDate.HasValue ? x.ToDate.Value.ToString("dd/MM/yyyy") : "",
                                     FromDate = x.FromDate.HasValue ? x.FromDate.Value.ToString("dd/MM/yyyy") : "",
                                     Description = x.Description,
                                 }).ToPagedList(page, limit);
                    return model;
                }
                catch (Exception)
                {

                    throw;
                }

            });
        }
        // chi tiết quà tặng của trung tâm
        public async Task<VoucherModel> GetVoucherDetail(int ID)
        {
            try
            {
                var model = await (from a in DbContext.Gifts
                                   where a.ID.Equals(ID)
                                   && a.IsActive.Equals(SystemParam.ACTIVE)
                                   select new VoucherModel
                                   {
                                       ID = a.ID,
                                       Name = a.Name,
                                       Point = a.Point,
                                       EndDate = a.ToDate.HasValue ? a.ToDate.Value.ToString("dd/MM/yyyy") : "",
                                       FromDate = a.FromDate.HasValue ? a.FromDate.Value.ToString("dd/MM/yyyy") : "",
                                       Description = a.Description,
                                       UrlImage = a.UrlImage,
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<ListGiftNews>> GetListGift()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from x in DbContext.Gifts
                                 where x.IsActive == SystemParam.ACTIVE && x.Type == SystemParam.TYPE_GIFT_VOURCHER && DateTime.Now < x.ToDate
                                 && x.Status == SystemParam.ACTIVE && x.GiftCodeQRs.Where(g => g.Status.Equals(SystemParam.STATUS_GIFTCODE_NOT_EXCHANGE)).Count() > 0
                                 select new ListGiftNews
                                 {
                                     ID = x.ID,
                                     Name = x.Name,
                                 }).ToListAsync();
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
