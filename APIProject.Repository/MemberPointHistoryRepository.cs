using APIProject.Common.DTOs.MenberPointHistory;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Utils;
using PagedList.Core;
using APIProject.Common.DTOs.Bill;
using Microsoft.EntityFrameworkCore;
using APIProject.Service.Utils;

namespace APIProject.Repository
{
    public class MemberPointHistoryRepository : BaseRepository<MemberPointHistory>, IMemberPointHistoryRepository
    {
        public MemberPointHistoryRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IPagedList<MemberPointHistoryModel>> GetListPointHistory(int page, int limit, int? type, int? customerID, string eventName, string fromDate, string toDate)
        {
            try
            {
                var fDate = Util.ConvertFromDate(fromDate);
                var tDate = Util.ConvertToDate(toDate);
                return await Task.Run(() =>
                {
                    var model = (from ph in DbContext.PointHistories
                                 where ph.CustomerID.Equals(customerID) && ph.IsActive.Equals(SystemParam.ACTIVE)
                                 && (!String.IsNullOrEmpty(eventName) ? ph.EventParticipant.News.Title.Contains(eventName) : true)
                                 && (fDate.HasValue ? ph.CreatedDate >= fDate : true)
                                 && (tDate.HasValue ? ph.CreatedDate <= tDate : true)
                                 && ph.Type.Equals(type)
                                 orderby ph.CreatedDate descending
                                 select new MemberPointHistoryModel
                                 {
                                     ID = ph.ID,
                                     EventName = ph.EventParticipant.News.Title,
                                     Point = ph.Point,
                                     StaffName = DbContext.Customers.Where(x => x.ID == ph.EventParticipant.StaffID).Select(x => x.Name).FirstOrDefault(),
                                     CountBill = ph.EventParticipant.Bills.Count(),
                                     TotalMoney = ph.EventParticipant.TotalMoney,
                                     Balance = ph.Balance,
                                     CreateDate = ph.CreatedDate,
                                 }).ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IPagedList<ListBillModel>> GetPointHistoryDetail(int page, int limit, int ID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from ph in DbContext.PointHistories
                                 join ev in DbContext.EventParticipants on ph.EventParticipantID equals ev.ID
                                 join b in DbContext.Bills on ev.ID equals b.EventParticipantID
                                 where ph.IsActive.Equals(SystemParam.ACTIVE)
                                 && ph.ID.Equals(ID)
                                 && b.IsActive.Equals(SystemParam.ACTIVE)
                                 select new ListBillModel
                                 {
                                     ID = b.ID,
                                     Code = b.Code,
                                     Image = b.ImageUrl,
                                     Price = b.Price,
                                     StallName = b.Stall.Name,
                                 }).OrderByDescending(p => p.ID).ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IPagedList<MemberChangeHistoryModel>> GetListChangeHistory(int page, int limit, int? type, int? customerID, string searchKey, string fromDate, string toDate)
        {
            try
            {
                var fDate = Util.ConvertFromDate(fromDate);
                var tDate = Util.ConvertToDate(toDate);
                return await Task.Run(() =>
                {
                    var model = (from ph in DbContext.PointHistories
                                 where ph.CustomerID.Equals(customerID) && ph.IsActive.Equals(SystemParam.ACTIVE)
                                 && (!String.IsNullOrEmpty(searchKey) ? ph.Gift.Name.Contains(searchKey) : true)
                                 && (fDate.HasValue ? ph.CreatedDate >= fDate : true)
                                 && (tDate.HasValue ? ph.CreatedDate <= tDate : true)
                                 && ph.Type.Equals(type)
                                 orderby ph.CreatedDate descending
                                 select new MemberChangeHistoryModel
                                 {
                                     ID = ph.ID,
                                     GiftID = ph.GiftID,
                                     GiftName = ph.Gift.Name,
                                     Point = ph.Point,
                                     Type = ph.Gift.Type,
                                     Balance = ph.Balance,
                                     CreateDate = ph.CreatedDate,
                                 }).ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IPagedList<PointHistoryModel>> GetListPointHistoryModel(int page, int limit, int? type, int customerID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from ph in DbContext.PointHistories
                                 where ph.CustomerID.Equals(customerID) && ph.IsActive.Equals(SystemParam.ACTIVE)
                                 && (type.HasValue ? ph.Type.Equals(type.Value) : true)
                                 orderby ph.CreatedDate descending
                                 select new PointHistoryModel
                                 {
                                     ID = ph.ID,
                                     CreatedDate = ph.CreatedDate.ToString("hh:mm dd/MM/yyyy"),
                                     Point = ph.Point,
                                     Type = ph.Type,
                                     Balance = ph.Balance,
                                     ImageUrl = ph.EventParticipantID.HasValue ? ph.EventParticipant.News.UrlImage : ph.GiftID.HasValue ? ph.Gift.UrlImage : "",
                                     Title = ph.Title
                                 }).ToPagedList(page, limit);
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
