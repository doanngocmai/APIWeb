using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using APIProject.Service.Models.Customer;
using APIProject.Service.Utils;
using PagedList.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APIProject.Common.Utils;
using APIProject.Common.DTOs.Staff;
using APIProject.Common.DTOs.Customer;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Repository
{
    public class StaffRepository:BaseRepository<Customer>, IStaffRepository
    {
        public StaffRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IPagedList<StaffModel>> GetListStaff(int page, int limit, int? status, string searchKey, int? searchProvince)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var model = (from cus in DbContext.Customers
                                 where cus.IsActive.Equals(SystemParam.ACTIVE) && cus.Role.Equals(SystemParam.ROLE_STAFF)
                                 && (!String.IsNullOrEmpty(searchKey) ? (cus.Name.Contains(searchKey) || cus.Phone.Contains(searchKey)) : true)
                                 && (searchProvince.HasValue ? cus.ProvinceID.Equals(searchProvince)  : true)
                                 && (status.HasValue ? cus.Status.Equals(status) : true)
                                 orderby cus.CreatedDate descending
                                 select new StaffModel
                                 {
                                     ID = cus.ID,
                                     Name = cus.Name,
                                     Phone = cus.Phone,
                                     Status = cus.Status,
                                     CreateDate = cus.CreatedDate,
                                     Email = cus.Email,
                                     DistrictName = cus.District.Name,
                                     DistrictID = cus.DistrictID,
                                     Gender = cus.Gender,
                                     ProvinceName = cus.Province.Name,
                                     ProvinceID = cus.ProvinceID,
                                     WardName = cus.Ward.Name,
                                     WardID = cus.WardID,
                                     Address = cus.Address,
                                     DOB = cus.DOB,
                                 }).AsQueryable().ToPagedList(page, limit);
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
