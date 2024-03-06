using APIProject.Common.DTOs.Category;
using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using PagedList.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Utils;
using Microsoft.EntityFrameworkCore;
using APIProject.Common.DTOs.Bill;

namespace APIProject.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IPagedList<CategoryModel>> GetListCategory(int page, int limit)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from ca in DbContext.Categorys
                                 where ca.IsActive.Equals(SystemParam.ACTIVE)
                                 orderby ca.CreatedDate descending
                                 select new CategoryModel
                                 {
                                     ID = ca.ID,
                                     Name = ca.Name,
                                     ImageUrl = ca.ImageUrl,
                                     Status = ca.Status,
                                 }).ToPagedList(page, limit);
                    return model;
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CategoryModel>> GetListCategory()
        {
            try
            {
                var model = await (from ca in DbContext.Categorys
                                   where ca.IsActive.Equals(SystemParam.ACTIVE) && ca.Status.Equals(SystemParam.ACTIVE)
                                   orderby ca.CreatedDate descending
                                   select new CategoryModel
                                   {
                                       ID = ca.ID,
                                       Name = ca.Name,
                                       Status = ca.Status,
                                       ImageUrl = ca.ImageUrl,
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<SumBillCategoryModel>> GetSumBillCategory(int TypeOrder, int? EventID)//đang làm thêm lọc chiến dịch
        {
            try
            {
                List<SumBillCategoryModel> model;
                var News = await DbContext.News.Where(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.ID.Equals(EventID)).FirstOrDefaultAsync();
                if (EventID == null)
                {
                    model = await (from cate in DbContext.Categorys
                                   where cate.IsActive.Equals(SystemParam.ACTIVE) && cate.Status.Equals(SystemParam.Status_Activate)
                                   select new SumBillCategoryModel
                                   {
                                       ID = cate.ID,
                                       Name = cate.Name,
                                       AverageBillPrice = cate.Stalls.Count > 0 ? Math.Ceiling(cate.Stalls.Average(x => x.Bills.Count > 0 ? x.Bills.Sum(b => b.Price) : 0)) : 0
                                   }).OrderBy(x => TypeOrder == SystemParam.TYPE_TOP_STALL_BILL_LEAST ? x.AverageBillPrice : -x.AverageBillPrice).Take(SystemParam.LIMIT_DEFAULT).ToListAsync();
                }
                else
                {
                    model = await (from cate in DbContext.Categorys
                                   where cate.IsActive.Equals(SystemParam.ACTIVE) && cate.Status.Equals(SystemParam.Status_Activate)
                                   select new SumBillCategoryModel
                                   {
                                       ID = cate.ID,
                                       Name = cate.Name,
                                       AverageBillPrice = News != null ? (cate.Stalls.Count > 0 ? Math.Ceiling(cate.Stalls.Average(x => x.Bills.Count > 0 ? x.Bills.Where(x => x.EventParticipant.NewsID == EventID).Sum(b => b.Price) : 0)) : 0) : 0
                                   }).OrderBy(x => TypeOrder == SystemParam.TYPE_TOP_STALL_BILL_LEAST ? x.AverageBillPrice : -x.AverageBillPrice).Take(SystemParam.LIMIT_DEFAULT).ToListAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<SumBillCategoryModel>> GetAllSumBillCategory()//đang làm thêm lọc chiến dịch
        {
            try
            {
                List<SumBillCategoryModel> model;
                model = await (from cate in DbContext.Categorys
                               where cate.IsActive.Equals(SystemParam.ACTIVE) && cate.Status.Equals(SystemParam.Status_Activate)
                               select new SumBillCategoryModel
                               {
                                   ID = cate.ID,
                                   Name = cate.Name,
                                   AverageBillPrice = cate.Stalls.Count > 0 ? Math.Ceiling(cate.Stalls.Average(x => x.Bills.Count > 0 ? x.Bills.Sum(b => b.Price) : 0)) : 0
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
