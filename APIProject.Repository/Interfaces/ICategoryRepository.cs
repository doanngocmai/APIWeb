using APIProject.Common.DTOs.Bill;
using APIProject.Common.DTOs.Category;
using APIProject.Domain.Models;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Task<IPagedList<CategoryModel>> GetListCategory(int Page, int Limit);
        Task<List<CategoryModel>> GetListCategory();
        Task<IList<SumBillCategoryModel>> GetSumBillCategory(int TypeOrder, int? EventID);
        Task<IList<SumBillCategoryModel>> GetAllSumBillCategory();
    }
}
