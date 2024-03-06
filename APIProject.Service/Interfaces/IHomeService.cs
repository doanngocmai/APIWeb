using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IHomeService
    {
        Task<JsonResultModel> GetHome();
        Task<JsonResultModel> GetHomeStaff();
        Task<JsonResultModel> OverView();
        Task<JsonResultModel> CheckTimezone();
    }
}
