using APIProject.Common.DTOs.Stall;
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
using Microsoft.EntityFrameworkCore;
using APIProject.Common.DTOs.Customer;
using APIProject.Common.DTOs.News;
using APIProject.Common.DTOs.Bill;

namespace APIProject.Repository
{
    public class StallRepository : BaseRepository<Stall>, IStallRepository
    {
        public StallRepository(ApplicationDbContext dbContext) : base(dbContext) { }
        public async Task<IPagedList<StallModel>> GetListStall(int page, int limit, int? floor, int? status, string searchKey, int? CategoryID)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from st in DbContext.Stalls
                                 where st.IsActive.Equals(SystemParam.ACTIVE)
                                 && (!string.IsNullOrEmpty(searchKey) ? (st.Name.Contains(searchKey) || st.Phone.Contains(searchKey)) : true)
                                 && (status.HasValue ? st.Status.Equals(status) : true)
                                 && (floor.HasValue ? st.Floor.Equals(floor) : true)
                                 && (CategoryID.HasValue ? st.CategoryID.Equals(CategoryID) : true)
                                 orderby st.CreatedDate descending
                                 select new StallModel
                                 {
                                     ID = st.ID,
                                     Name = st.Name,
                                     Code = st.Code,
                                     Phone = st.Phone,
                                     Floor = st.Floor,
                                     Logo = st.Logo,
                                     Index = st.Index,
                                     Status = st.Status,
                                     CategoryID = st.CategoryID,
                                     CategoryName = st.Category.Name,
                                     CreatedDate = st.CreatedDate.ToString("dd/MM/yyyy")
                                 }).ToPagedList(page, limit);
                    return model;
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<StallModel>> GetListStall(int page, int limit, int? floor, string searchKey, int? categoryID, int? orderBy)
        {
            try
            {
                var model = (from st in DbContext.Stalls
                             where st.IsActive.Equals(SystemParam.ACTIVE)
                             && st.Status.Equals(SystemParam.ACTIVE)
                             && (!string.IsNullOrEmpty(searchKey) ? (st.Name.Contains(searchKey) || st.Phone.Contains(searchKey)) : true)
                             && (floor.HasValue ? st.Floor.Equals(floor) : true)
                             && (categoryID.HasValue ? st.CategoryID.Equals(categoryID) : true)
                             select new StallModel
                             {
                                 ID = st.ID,
                                 Name = st.Name,
                                 Code = st.Code,
                                 Phone = st.Phone,
                                 Floor = st.Floor,
                                 Logo = st.Logo,
                                 Status = st.Status,
                                 CategoryID = st.CategoryID,
                                 Index = st.Index,
                                 CategoryName = st.Category.Name,
                                 CreatedDate = st.CreatedDate.ToString("dd/MM/yyyy")
                             }).OrderBy(x => x.Index.HasValue ? x.Index.Value : 0).ThenByDescending(x => x.ID).ToPagedList(page, limit);
                if (orderBy.HasValue)
                {
                    if (orderBy.Value == SystemParam.SORT_ASCENDING)
                        return model.OrderBy(x => x.Name).ToList();
                    else if (orderBy.Value == SystemParam.SORT_DESCENDING)
                        return model.OrderByDescending(x => x.Name).ToList();
                    else
                        return model.OrderBy(x => x.Index).ToList();
                }
                return model.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<StallModel>> GetListStall()
        {
            try
            {
                var model = await (from st in DbContext.Stalls
                                   where st.IsActive.Equals(SystemParam.ACTIVE) && st.Status.Equals(SystemParam.ACTIVE)
                                   select new StallModel
                                   {
                                       ID = st.ID,
                                       Name = st.Name,
                                       Code = st.Code,
                                       Phone = st.Phone,
                                       Floor = st.Floor,
                                       Logo = st.Logo,
                                       Status = st.Status,
                                       CategoryID = st.CategoryID,
                                       Index = st.Index,
                                       CategoryName = st.Category.Name,
                                       CreatedDate = st.CreatedDate.ToString("dd/MM/yyyy")
                                   }).OrderBy(x => x.Name).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<StallDetailModel> GetStallDetail(int ID)
        {
            try
            {
                var model = await (from st in DbContext.Stalls
                                   where st.IsActive.Equals(SystemParam.ACTIVE) && st.ID.Equals(ID)
                                   select new StallDetailModel
                                   {
                                       ID = st.ID,
                                       Name = st.Name,
                                       Code = st.Code,
                                       Phone = st.Phone,
                                       Floor = st.Floor,
                                       Logo = st.Logo,
                                       Index = st.Index,
                                       Status = st.Status,
                                       LinkFB = st.LinkFB,
                                       LinkWeb = st.LinkWeb,
                                       Description = st.Description,
                                       CategoryID = st.CategoryID,
                                       CategoryName = st.Category.Name,
                                       ListPromotion = (from relatedstall in DbContext.RelatedStalls
                                                        join news in DbContext.News on relatedstall.NewsID equals news.ID
                                                        where news.IsActive.Equals(SystemParam.ACTIVE) && st.ID.Equals(relatedstall.StallID)
                                                        orderby news.CreatedDate descending
                                                        select new NewsModel
                                                        {
                                                            ID = news.ID,
                                                            Title = news.Title,
                                                            Type = news.Type,
                                                            TypePost = news.TypePost,
                                                            Status = news.Status,
                                                            UrlImage = news.UrlImage,
                                                            StartDate = news.StartDate.HasValue ? news.StartDate.Value.ToString(SystemParam.CONVERT_DATETIME) : "",
                                                            EndDate = news.EndDate.HasValue ? news.EndDate.Value.ToString(SystemParam.CONVERT_DATETIME) : "",
                                                            CreateDate = news.CreatedDate,
                                                            Index = news.Index,
                                                            IsBanner = news.IsBanner
                                                        }).ToList(),
                                       ListOtherStall = (from stall in DbContext.Stalls
                                                         where stall.IsActive.Equals(SystemParam.ACTIVE) && stall.Floor.Equals(st.Floor) && stall.Status.Equals(SystemParam.ACTIVE) && stall.ID != st.ID
                                                         orderby stall.ID descending
                                                         select new OtherStall
                                                         {
                                                             ID = stall.ID,
                                                             CategoryName = stall.Category.Name,
                                                             Floor = stall.Floor,
                                                             Logo = stall.Logo,
                                                             Name = stall.Name
                                                         }).ToList()
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<StallModel>> GetStallRelated(int ID)
        {
            try
            {
                var data = await GetFirstOrDefaultAsync(x => x.ID == ID);
                if (data == null)
                {
                    return new List<StallModel>();
                }
                var model = await (from st in DbContext.Stalls
                                   where st.IsActive.Equals(SystemParam.ACTIVE)
                                   && st.ID != ID && st.Floor.Equals(data.Floor)
                                   && st.Status.Equals(SystemParam.ACTIVE)
                                   select new StallModel
                                   {
                                       ID = st.ID,
                                       Name = st.Name,
                                       Code = st.Code,
                                       Phone = st.Phone,
                                       Floor = st.Floor,
                                       Logo = st.Logo,
                                       Index = st.Index,
                                       Status = st.Status,
                                       CategoryID = st.CategoryID,
                                       CategoryName = st.Category.Name,
                                   }).OrderBy(x => x.Index.HasValue ? x.Index.Value : 0).ThenByDescending(x => x.ID).Take(SystemParam.LIMIT_DEFAULT).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IList<SumBillStallModel>> GetSumBillStall(int TypeOrder, int? EventID)//đang làm thêm lọc chiến dịch
        {
            try
            {
                List<SumBillStallModel> model;
                var News = await DbContext.News.Where(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.ID.Equals(EventID)).FirstOrDefaultAsync();
                if (EventID == null)
                {
                    model = await (from stall in DbContext.Stalls
                                   where stall.IsActive.Equals(SystemParam.ACTIVE) && stall.Status.Equals(SystemParam.Status_Activate)
                                   select new SumBillStallModel
                                   {
                                       ID = stall.ID,
                                       Name = stall.Name,
                                       SumBill = stall.Bills.Count()
                                   }).OrderBy(x => TypeOrder == SystemParam.TYPE_TOP_STALL_BILL_LEAST ? x.SumBill : -x.SumBill).Take(SystemParam.LIMIT_DEFAULT).ToListAsync();
                }
                else
                {
                    model = await (from stall in DbContext.Stalls
                                   where stall.IsActive.Equals(SystemParam.ACTIVE) && stall.Status.Equals(SystemParam.Status_Activate)
                                   select new SumBillStallModel
                                   {
                                       ID = stall.ID,
                                       Name = stall.Name,
                                       SumBill = News != null ? stall.Bills.Where(a => a.EventParticipant.NewsID.Equals(News.ID)).Count() : 0
                                   }).OrderBy(x => TypeOrder == SystemParam.TYPE_TOP_STALL_BILL_LEAST ? x.SumBill : -x.SumBill).Take(SystemParam.LIMIT_DEFAULT).ToListAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<SumBillStallModel>> GetAllSumBillStall()//đang làm thêm lọc chiến dịch
        {
            try
            {
                List<SumBillStallModel> model;
                model = await (from stall in DbContext.Stalls
                               where stall.IsActive.Equals(SystemParam.ACTIVE) && stall.Status.Equals(SystemParam.Status_Activate)
                               select new SumBillStallModel
                               {
                                   ID = stall.ID,
                                   Name = stall.Name,
                                   SumBill = stall.Bills.Count()
                               }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
