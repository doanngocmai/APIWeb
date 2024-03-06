using APIProject.Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IStatisticService
    {
        Task<JsonResultModel> GetListSameFilter(int? Type);
        Task<JsonResultModel> FilterEventParticipant(string Fromdate, string Todate);
        Task<JsonResultModel> GetCustomerCampaign(int ID);
        Task<JsonResultModel> GetCountGiftBills(string Fromdate, string Todate, int? EventID);
        Task<JsonResultModel> GetTotalGiftBills(string Fromdate, string Todate, int? EventID);
        Task<JsonResultModel> ExportExcelTotalGiftBills(string Fromdate, string Todate, IWebHostEnvironment webHostEnvironment, HttpContext context);
        Task<JsonResultModel> ExportExcelCountGiftBills(string Fromdate, string Todate, IWebHostEnvironment webHostEnvironment, HttpContext context);
    }
}
