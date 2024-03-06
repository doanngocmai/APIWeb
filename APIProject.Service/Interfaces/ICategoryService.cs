using APIProject.Common.DTOs.Category;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<JsonResultModel> GetListCategory();
        Task<JsonResultModel> GetListCategory(int page, int limit);
        Task<JsonResultModel> CreateCategory(CreateCategoryModel input);
        Task<JsonResultModel> UpdateCategory(CategoryModel input);
        Task<JsonResultModel> DeleteCategory(int ID);
        Task<JsonResultModel> GetSumBillCategory(int TypeOrder, int? EventID);//Thống kê bé lớn lớn bé số lượng bill trong ngành hàng
        Task<JsonResultModel> ExportExcelSumBillCategory(IWebHostEnvironment webHostEnvironment, HttpContext context);
    }
}
