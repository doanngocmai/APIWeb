using APIProject.Common.DTOs.Stall;
using APIProject.Domain.Models;
using APIProject.Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IStallService
    {
        Task<JsonResultModel> GetListStall(int page, int limit, int? Floor, int? status, string searchKey, int? type);
        Task<JsonResultModel> GetListStall(int? page, int? limit, int? Floor, string searchKey, int? categoryID, int? orderBy);
        Task<JsonResultModel> GetStallRelated(int ID);
        Task<JsonResultModel> GetStallDetail(int ID);
        Task<JsonResultModel> GetSumBillStall(int TypeOrder,int? EventID);//Thống kê bé lớn lớn bé số lượng bill trong gian hàng
        Task<JsonResultModel> ChangeStatus(int customerID);
        Task<JsonResultModel> CreateStall(CreateStallModel input);
        Task<JsonResultModel> UpdateStall(UpdateStallModel input);
        Task<JsonResultModel> DeleteStall(int ID);
        Task<JsonResultModel> ExportExcelSumBillStall(IWebHostEnvironment webHostEnvironment, HttpContext context);
    }
}
