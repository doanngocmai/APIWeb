using APIProject.Common.DTOs.Staff;
using APIProject.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Interfaces
{
    public interface IStaffService
    {
        Task<JsonResultModel> GetListStaff(int page, int limit, int? status, string searchKey, int? searchProvince);
        Task<JsonResultModel> CreateStaff(AddStaffModel input);
        Task<JsonResultModel> UpdateStaff(UpdateStaffModel input);
        Task<JsonResultModel> DeleteStaff(int ID);
        Task<JsonResultModel> ChangeStatus(int ID);
    }
}
