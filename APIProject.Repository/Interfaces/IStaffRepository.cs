using APIProject.Common.DTOs.Customer;
using APIProject.Common.DTOs.Staff;
using APIProject.Domain.Models;
using APIProject.Service.Models.Customer;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{
    public interface IStaffRepository:IRepository<Customer>
    {
        Task<IPagedList<StaffModel>> GetListStaff(int page, int limit, int? status, string searchKey, int? searchProvince);
    }
}
