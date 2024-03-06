


/*------------------------------------------------
 * AUthor   : NGuyễn Viết Minh Tiến
 * DateTime : 15/12/2021
 * Edit     : Chưa chỉnh Sửa
 * Content  : hàm đếm số điện thoại, hàm đếm Email
 * -----------------------------------------------*/

using APIProject.Domain;
using APIProject.Domain.Models;
using APIProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using APIProject.Service.Utils;
using APIProject.Service.Models.Customer;
using APIProject.Common.Utils;
using APIProject.Common.DTOs.Customer;
using APIProject.Service.Models;
using System.Security.Cryptography.X509Certificates;
using Sentry.PlatformAbstractions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using APIProject.Common.DTOs.UsageFrequency;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Net.WebSockets;
using System.Globalization;
using Microsoft.AspNetCore.Http.Connections;

namespace APIProject.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<int> CountCustomer()
        {
            try
            {
                return await DbContext.Customers.Where(x => x.IsActive.Equals(SystemParam.ACTIVE)).CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CustomerAppModel> GetCustomerInfo(Customer cus)
        {
            try
            {
                var model = new CustomerAppModel
                {
                    ID = cus.ID,
                    Phone = cus.Phone,
                    Name = cus.Name,
                    Token = cus.Token,
                    DeviceID = cus.DeviceID,
                    Email = cus.Email,
                    ProvinceID = cus.ProvinceID,
                    DistrictID = cus.DistrictID,
                    DOB = cus.DOB,
                    Job = cus.Job,
                    QRCode = await DbContext.QRCodes.Where(x => x.Phone.Equals(cus.Phone) && x.IsActive.Equals(SystemParam.ACTIVE)).OrderByDescending(x => x.CreatedDate).Select(x => x.Code).FirstOrDefaultAsync(),
                    IdentityNumber = cus.IdentityNumber,
                    Avatar = cus.Avatar,
                    Address = cus.Address,
                    WardID = cus.WardID,
                    Gender = cus.Gender,
                    Point = cus.Point,
                    Status = cus.Status,
                    Role = cus.Role
                };
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CountEmailOfCustomer(string Email)
        {
            return await DbContext.Customers.Where(x => x.Email.Equals(Email)).CountAsync();
        }
        public async Task<int> CountPhoneOfCustomer(string Phone)
        {
            return await DbContext.Customers.Where(x => x.Phone.Equals(Phone)).CountAsync();
        }

        public async Task<IPagedList<CustomerWebModel>> GetCustomers(int page, int limit, int? status, string searchKey, string fromDate, string toDate, int? originCustomer)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);
                    var model = (from cus in DbContext.Customers
                                 where cus.IsActive.Equals(SystemParam.ACTIVE) && cus.Role.Equals(SystemParam.ROLE_CUSTOMER)
                                 && (!string.IsNullOrEmpty(searchKey) ? (cus.Name.Contains(searchKey) || cus.Phone.Contains(searchKey)) : true)
                                 && (fd.HasValue ? cus.CreatedDate >= fd : true)
                                 && (td.HasValue ? cus.CreatedDate <= td : true)
                                 && (status.HasValue ? cus.Status.Equals(status) : true)
                                 && (originCustomer.HasValue ? cus.OriginCustomer == originCustomer : true)
                                 orderby cus.CreatedDate descending
                                 select new CustomerWebModel
                                 {
                                     ID = cus.ID,
                                     Name = cus.Name,
                                     Phone = cus.Phone,
                                     Status = cus.Status,
                                     LastLoginDate = cus.LastLoginDate.HasValue ? cus.LastLoginDate.Value.ToString(SystemParam.CONVERT_DATETIME_HAVE_HOUR) : "",
                                     OriginCustomer = cus.OriginCustomer,
                                     CreateDate = cus.CreatedDate,
                                     EventParticipantCount = cus.EventParticipants.Count(),
                                     Email = cus.Email,
                                     District = cus.District.Name,
                                     Gender = cus.Gender,
                                     Province = cus.Province.Name,
                                     Ward = cus.Ward.Name,
                                     Address = cus.Address,
                                 }).AsQueryable().ToPagedList(page, limit);
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CustomerDetailModel> GetCustomerDetail(int ID)
        {
            try
            {
                var model = await (from cus in DbContext.Customers
                                   where cus.IsActive.Equals(SystemParam.ACTIVE) && cus.ID.Equals(ID)
                                   select new CustomerDetailModel
                                   {
                                       ID = cus.ID,
                                       Name = cus.Name,
                                       Phone = cus.Phone,
                                       Email = cus.Email,
                                       IdentityNumber = cus.IdentityNumber,
                                       Gender = cus.Gender,
                                       Address = cus.Address,
                                       WardID = cus.WardID,
                                       DistrictID = cus.DistrictID,
                                       ProvinceID = cus.ProvinceID,
                                       DOB = cus.DOB,
                                       ProvinceName = cus.ProvinceID.HasValue ? cus.Province.Name : "",
                                       DistrictName = cus.DistrictID.HasValue ? cus.District.Name : "",
                                       WardName = cus.WardID.HasValue ? cus.Ward.Name : "",
                                       Job = cus.Job,
                                       Point = cus.Point
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IPagedList<CustomerNumberEvent>> GetNumberCustomerEvent(int page, int limit, string searchKey, int? EventID, string fromDate, string toDate)
        {
            try
            {
                //
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);
                    var Model = (from a in DbContext.News
                                 where a.IsActive.Equals(SystemParam.ACTIVE)
                                         && a.Type.Equals(SystemParam.TYPE_NEWS_EVENT)
                                         && a.TypePost.Equals(SystemParam.NEWS_TYPEPOST_POSTED)
                                         && (!string.IsNullOrEmpty(searchKey) ? a.Title.Contains(searchKey) : true)
                                         && (EventID.HasValue ? a.ID.Equals(EventID) : true)
                                         && (fd.HasValue ? a.StartDate <= td : true)
                                         && (td.HasValue ? a.EndDate >= fd : true)
                                 select new CustomerNumberEvent
                                 {
                                     ID = a.ID,
                                     Name = a.Title,
                                     FromDate = a.StartDate,
                                     ToDate = a.EndDate,
                                     Number = a.EventParticipants.Count()
                                 }).ToPagedList(page, limit);
                    return Model;
                });
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public async Task<IPagedList<CustomerEventDetail>> CustomerEventDetail(int page, int limit, string searchKey, int ID, string fromDate, string toDate)
        {

            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);
                    var model = (from a in DbContext.EventParticipants
                                 where a.IsActive.Equals(SystemParam.ACTIVE)
                                         && a.NewsID.Equals(ID)
                                         && (!string.IsNullOrEmpty(searchKey) ? a.Phone.Contains(searchKey) || a.Name.Contains(searchKey) : true)
                                         && (fd.HasValue ? a.CreatedDate >= fd : true)
                                         && (td.HasValue ? a.CreatedDate <= td : true)
                                 select new CustomerEventDetail
                                 {
                                     ID = a.ID,
                                     Name = a.Customer.Name,
                                     Phone = a.Customer.Phone,
                                     CreatedDate = a.CreatedDate
                                 }).ToPagedList(page, limit);

                    return model;
                });

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<PercentageCustomers>> GetListPercentageCustomer(int? EventID)
        {
            try
            {
                List<PercentageCustomers> model;
                var SumDis = DbContext.Customers.Where(x => x.DistrictID.Equals(SystemParam.DISTRIC_ID_LB)).Count();
                if (EventID != null)
                {
                    var SumDis1 = DbContext.EventParticipants.Where(x => x.DistrictID.Equals(SystemParam.DISTRIC_ID_LB) && x.NewsID.Equals(EventID)).Count();
                    if (SumDis1 == 0)
                    {
                        model = await (from a in DbContext.Wards
                                       where a.DistrictID.Equals(SystemParam.DISTRIC_ID_LB)
                                       select new PercentageCustomers
                                       {
                                           ID = a.ID,
                                           Name = a.Name,
                                           Percent = 0
                                       }).ToListAsync();
                    }
                    else
                    {
                        model = await (from a in DbContext.Wards
                                       where a.DistrictID.Equals(SystemParam.DISTRIC_ID_LB)
                                       select new PercentageCustomers
                                       {
                                           ID = a.ID,
                                           Name = a.Name,
                                           Percent = (from i in DbContext.EventParticipants where i.WardID == a.ID && i.NewsID.Equals(EventID) select i.ID).Count() / SumDis1 * 100
                                       }).ToListAsync();
                    }
                }
                else
                {
                    model = await (from a in DbContext.Wards
                                   where a.DistrictID.Equals(SystemParam.DISTRIC_ID_LB)
                                   select new PercentageCustomers
                                   {
                                       ID = a.ID,
                                       Name = a.Name,
                                       Percent = (from i in DbContext.Customers where i.WardID == a.ID select i.ID).Count() / SumDis * 100
                                   }).ToListAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<PercentageGenderCustomer>> GetListPercentageGenderCustomer(int? EventID)
        {
            try
            {
                List<PercentageGenderCustomer> model;
                if (EventID != null)
                {
                    var Boy1 = DbContext.EventParticipants.Where(x => x.Gender.Equals(SystemParam.IDBoy) && x.IsActive.Equals(SystemParam.ACTIVE) && x.NewsID.Equals(EventID)).Count();
                    var Girl1 = DbContext.EventParticipants.Where(c => c.Gender.Equals(SystemParam.IDGirl) && c.IsActive.Equals(SystemParam.ACTIVE) && c.NewsID.Equals(EventID)).Count();
                    var SumCustomer = Boy1 + Girl1;

                    if (SumCustomer == 0)
                    {
                        model = new List<PercentageGenderCustomer>
                        {
                            new PercentageGenderCustomer{Gender=SystemParam.Gender_Boy, Precent=0},
                            new PercentageGenderCustomer{Gender=SystemParam.Gender_Girl, Precent=0}
                        }.ToList();
                    }
                    else
                    {
                        var c = (double)Boy1 / SumCustomer * 100;
                        var d = (double)Girl1 / SumCustomer * 100;
                        model = new List<PercentageGenderCustomer>
                        {
                            new PercentageGenderCustomer{Gender=SystemParam.Gender_Boy, Precent=c},
                            new PercentageGenderCustomer{Gender=SystemParam.Gender_Girl, Precent=d}
                        }.ToList();
                    }

                }
                else
                {
                    var Boy = DbContext.Customers.Where(x => x.Gender.Equals(SystemParam.IDBoy) && x.IsActive.Equals(SystemParam.ACTIVE) && x.Status.Equals(SystemParam.Status_Activate)).Count();
                    var Girl = DbContext.Customers.Where(c => c.Gender.Equals(SystemParam.IDGirl) && c.IsActive.Equals(SystemParam.ACTIVE) && c.Status.Equals(SystemParam.Status_Activate)).Count();
                    var SumCustomer = Boy + Girl;
                    var a = (double)Boy / SumCustomer * 100;
                    var b = (double)Girl / SumCustomer * 100;
                    model = new List<PercentageGenderCustomer>
                    {
                        new PercentageGenderCustomer{Gender=SystemParam.Gender_Boy, Precent=a},
                        new PercentageGenderCustomer{Gender=SystemParam.Gender_Girl, Precent=b}
                    }.ToList();
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<List<CustomerChannelPercentage>> GetListCustomerChannelPercentage(int? EventID)
        {
            try
            {
                List<CustomerChannelPercentage> model;
                if (EventID != null)
                {
                    var SumDis1 = DbContext.EventParticipants.Where(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.NewsID.Equals(EventID)).Count();
                    if (SumDis1 == 0)
                    {
                        model = await (from a in DbContext.EventChannels
                                       where a.IsActive.Equals(SystemParam.ACTIVE)
                                       && a.Status.Equals(SystemParam.ACTIVE)
                                       select new CustomerChannelPercentage
                                       {
                                           ID = a.ID,
                                           Name = a.Name,
                                           Percent = 0
                                       }).ToListAsync();
                    }
                    else
                    {
                        model = await (from a in DbContext.EventChannels
                                       where a.IsActive.Equals(SystemParam.ACTIVE)
                                       && a.Status.Equals(SystemParam.ACTIVE)
                                       select new CustomerChannelPercentage
                                       {
                                           ID = a.ID,
                                           Name = a.Name,
                                           Percent = (from i in DbContext.EventParticipants
                                                      where i.EventChannelID == a.ID
                                                      && i.NewsID == EventID
                                                      && i.IsActive.Equals(SystemParam.ACTIVE)
                                                      select i.ID).Count() / SumDis1 * 100
                                       }).ToListAsync();
                    }
                }
                else
                {
                    var SumDis = DbContext.EventParticipants.Where(x => x.IsActive.Equals(SystemParam.ACTIVE)).Count();
                    model = await (from a in DbContext.EventChannels
                                   where a.IsActive.Equals(SystemParam.ACTIVE)
                                   && a.Status.Equals(SystemParam.ACTIVE)
                                   select new CustomerChannelPercentage
                                   {
                                       ID = a.ID,
                                       Name = a.Name,
                                       Percent = (from i in DbContext.EventParticipants
                                                  where i.EventChannelID == a.ID
                                                  && i.IsActive.Equals(SystemParam.ACTIVE)
                                                  select i.ID).Count() / SumDis * 100
                                   }).ToListAsync();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CustomerPercentageProvinces>> GetListCustomerPercentageProvinces(int? EventID)
        {
            List<CustomerPercentageProvinces> model;
            if (EventID != null)
            {
                var Other = DbContext.EventParticipants.Where(a => a.IsActive.Equals(SystemParam.ACTIVE) && a.NewsID.Equals(EventID) && a.DistrictID != SystemParam.DISTRIC_ID_LB && a.DistrictID != SystemParam.DISTRIC_ID_GL && a.ProvinceID != SystemParam.PROVINCE_ID_HY && a.ProvinceID != SystemParam.PROVINCE_ID_BN).Count();
                var LongBien = DbContext.EventParticipants.Where(b => b.IsActive.Equals(SystemParam.ACTIVE) && b.NewsID.Equals(EventID) && b.DistrictID.Equals(SystemParam.DISTRIC_ID_LB)).Count();
                var Gialam = DbContext.EventParticipants.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.NewsID.Equals(EventID) && c.DistrictID.Equals(SystemParam.DISTRIC_ID_GL)).Count();
                var HungYen = DbContext.EventParticipants.Where(d => d.IsActive.Equals(SystemParam.ACTIVE) && d.NewsID.Equals(EventID) && d.ProvinceID.Equals(SystemParam.PROVINCE_ID_HY)).Count();
                var BacNinh = DbContext.EventParticipants.Where(e => e.IsActive.Equals(SystemParam.ACTIVE) && e.NewsID.Equals(EventID) && e.ProvinceID.Equals(SystemParam.PROVINCE_ID_BN)).Count();
                var SumCus = Other + LongBien + Gialam + HungYen + BacNinh;
                if (SumCus == 0)
                {
                    model = new List<CustomerPercentageProvinces>()
                    {
                        new CustomerPercentageProvinces{Name=SystemParam.DISTRIC_LB, Percent=0},
                        new CustomerPercentageProvinces{Name=SystemParam.OTHER, Percent=0},
                        new CustomerPercentageProvinces{Name=SystemParam.DISTRIC_GL, Percent=0},
                        new CustomerPercentageProvinces{Name=SystemParam.PROVINCE_HY, Percent=0},
                        new CustomerPercentageProvinces{Name=SystemParam.PROVINCE_BN, Percent=0},
                    }.ToList();
                }
                else
                {
                    var a1 = (double)LongBien / SumCus * 100;
                    var b1 = (double)Other / SumCus * 100;
                    var c1 = (double)Gialam / SumCus * 100;
                    var d1 = (double)HungYen / SumCus * 100;
                    var e1 = (double)BacNinh / SumCus * 100;
                    model = new List<CustomerPercentageProvinces>()
                    {
                        new CustomerPercentageProvinces{Name=SystemParam.DISTRIC_LB, Percent=a1},
                        new CustomerPercentageProvinces{Name=SystemParam.OTHER, Percent=b1},
                        new CustomerPercentageProvinces{Name=SystemParam.DISTRIC_GL, Percent=c1},
                        new CustomerPercentageProvinces{Name=SystemParam.PROVINCE_HY, Percent=d1},
                        new CustomerPercentageProvinces{Name=SystemParam.PROVINCE_BN, Percent=e1},
                    }.ToList();
                }

            }
            else
            {
                var Other = DbContext.Customers.Where(a => a.IsActive.Equals(SystemParam.ACTIVE) && a.Status.Equals(SystemParam.ACTIVE) && a.DistrictID != SystemParam.DISTRIC_ID_LB && a.DistrictID != SystemParam.DISTRIC_ID_GL && a.ProvinceID != SystemParam.PROVINCE_ID_HY && a.ProvinceID != SystemParam.PROVINCE_ID_BN).Count();
                var LongBien = DbContext.Customers.Where(b => b.IsActive.Equals(SystemParam.ACTIVE) && b.Status.Equals(SystemParam.ACTIVE) && b.DistrictID.Equals(SystemParam.DISTRIC_ID_LB)).Count();
                var Gialam = DbContext.Customers.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.Status.Equals(SystemParam.ACTIVE) && c.DistrictID.Equals(SystemParam.DISTRIC_ID_GL)).Count();
                var HungYen = DbContext.Customers.Where(d => d.IsActive.Equals(SystemParam.ACTIVE) && d.Status.Equals(SystemParam.ACTIVE) && d.ProvinceID.Equals(SystemParam.PROVINCE_ID_HY)).Count();
                var BacNinh = DbContext.Customers.Where(e => e.IsActive.Equals(SystemParam.ACTIVE) && e.Status.Equals(SystemParam.ACTIVE) && e.ProvinceID.Equals(SystemParam.PROVINCE_ID_BN)).Count();
                var SumCus = Other + LongBien + Gialam + HungYen + BacNinh;
                var a1 = (double)LongBien / SumCus * 100;
                var b1 = (double)Other / SumCus * 100;
                var c1 = (double)Gialam / SumCus * 100;
                var d1 = (double)HungYen / SumCus * 100;
                var e1 = (double)BacNinh / SumCus * 100;
                model = new List<CustomerPercentageProvinces>()
                {
                    new CustomerPercentageProvinces{Name=SystemParam.DISTRIC_LB, Percent=a1},
                    new CustomerPercentageProvinces{Name=SystemParam.OTHER, Percent=b1},
                    new CustomerPercentageProvinces{Name=SystemParam.DISTRIC_GL, Percent=c1},
                     new CustomerPercentageProvinces{Name=SystemParam.PROVINCE_HY, Percent=d1},
                     new CustomerPercentageProvinces{Name=SystemParam.PROVINCE_BN, Percent=e1},
                 }.ToList();
            }

            return model;
        }

        public async Task<List<CustomerPercentageAge>> GetListCustomerPercentageAge(int? EventID)
        {
            DateTime Today = DateTime.Now;
            List<CustomerPercentageAge> model;
            if (EventID != null)
            {
                var fifteen_twenty = DbContext.EventParticipants.Where(b => b.Customer.AgeType == SystemParam.Age_15_22 && b.NewsID.Equals(EventID)).Count();
                var twenty_twentyNine = DbContext.EventParticipants.Where(b => b.Customer.AgeType == SystemParam.Age_23_35 && b.NewsID.Equals(EventID)).Count();
                var thirty_thirty_Nine = DbContext.EventParticipants.Where(b => b.Customer.AgeType == SystemParam.Age_36_45 && b.NewsID.Equals(EventID)).Count();
                var on_fifty = DbContext.EventParticipants.Where(b => b.Customer.AgeType == SystemParam.Age_Above_45 && b.NewsID.Equals(EventID)).Count();
                var SumAge = fifteen_twenty + twenty_twentyNine + thirty_thirty_Nine + on_fifty;
                if (SumAge == 0)
                {
                    model = new List<CustomerPercentageAge>()
                    {
                        new CustomerPercentageAge{Name=SystemParam.Age_15_22_STR, Percent=0},
                        new CustomerPercentageAge{Name=SystemParam.Age_23_35_STR, Percent=0},
                        new CustomerPercentageAge{Name=SystemParam.Age_36_45_STR, Percent=0},
                        new CustomerPercentageAge{Name=SystemParam.Age_Above_45_STR, Percent=0},
                    }.ToList();
                }
                else
                {
                    var a = (double)fifteen_twenty / SumAge * 100;
                    var b = (double)twenty_twentyNine / SumAge * 100;
                    var c = (double)thirty_thirty_Nine / SumAge * 100;
                    var d = (double)on_fifty / SumAge * 100;
                    model = new List<CustomerPercentageAge>()
                    {
                        new CustomerPercentageAge{Name=SystemParam.Age_15_22_STR, Percent=a},
                        new CustomerPercentageAge{Name=SystemParam.Age_23_35_STR, Percent=b},
                        new CustomerPercentageAge{Name=SystemParam.Age_36_45_STR, Percent=c},
                        new CustomerPercentageAge{Name=SystemParam.Age_Above_45_STR, Percent=d},
                    }.ToList();
                }
            }
            else
            {
                var fifteen_twenty = DbContext.Customers.Where(b => b.AgeType == SystemParam.Age_15_22).Count();
                var twenty_twentyNine = DbContext.Customers.Where(b => b.AgeType == SystemParam.Age_23_35).Count();
                var thirty_thirty_Nine = DbContext.Customers.Where(b => b.AgeType == SystemParam.Age_36_45).Count();
                var on_fifty = DbContext.Customers.Where(b => b.AgeType == SystemParam.Age_Above_45).Count();
                var SumAge = fifteen_twenty + twenty_twentyNine + thirty_thirty_Nine + on_fifty;
                var a = (double)fifteen_twenty / SumAge * 100;
                var b = (double)twenty_twentyNine / SumAge * 100;
                var c = (double)thirty_thirty_Nine / SumAge * 100;
                var d = (double)on_fifty / SumAge * 100;
                model = new List<CustomerPercentageAge>()
                {
                        new CustomerPercentageAge{Name=SystemParam.Age_15_22_STR, Percent=a},
                        new CustomerPercentageAge{Name=SystemParam.Age_23_35_STR, Percent=b},
                        new CustomerPercentageAge{Name=SystemParam.Age_36_45_STR, Percent=c},
                        new CustomerPercentageAge{Name=SystemParam.Age_Above_45_STR, Percent=d},
                }.ToList();
            }

            return model;
        }

        public async Task<int> CountEventParticipant(DateTime FromDate, DateTime Todate)// func Count customer of EventParticipants
        {
            try
            {
                var SumCus = DbContext.EventParticipants.Where(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.CreatedDate >= FromDate && x.CreatedDate <= Todate).Count();
                return SumCus;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<long> SumBillAmount(DateTime FromDate, DateTime Todate)// func Total gift bill amount
        {
            try
            {
                long SumPriceBill = DbContext.Bills.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.CreatedDate >= FromDate && c.CreatedDate <= Todate).Sum(b => b.Price);
                return SumPriceBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> CountGiftBills(DateTime FromDate, DateTime Todate)// func Count gift bill
        {
            try
            {
                var SumCus = DbContext.Bills.Where(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.CreatedDate >= FromDate && x.CreatedDate <= Todate).Count();
                return SumCus;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> CountEventParticipantTime(DateTime Date) // func Count gift bill of Time
        {
            var SumCus = DbContext.EventParticipants.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.CreatedDate.Year == Date.Year && c.CreatedDate.Month == Date.Month && c.CreatedDate.Day == Date.Day && c.CreatedDate.Hour == Date.Hour).Count();
            return SumCus;
        }

        public async Task<int> CountCustomerCampaign(int ID, DateTime Date) // Func count customer in Campaign
        {
            try
            {
                var SumCus = DbContext.EventParticipants.Where(x => x.IsActive.Equals(SystemParam.ACTIVE) && x.NewsID.Equals(ID) && x.CreatedDate.Year == Date.Year && x.CreatedDate.Month == Date.Month && x.CreatedDate.Day == Date.Day).Count();
                return SumCus;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> CountSumBillCampaign(int EventID, DateTime Date)
        {
            try
            {

                int SumPriceBill = DbContext.Bills.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.EventParticipant.News.ID.Equals(EventID) && c.CreatedDate.Year == Date.Year && c.CreatedDate.Month == Date.Month && c.CreatedDate.Day == Date.Day).Count();
                return SumPriceBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> CountTotalPriceCampaign(int EventID, DateTime Date)
        {
            try
            {
                long SumPriceBill = DbContext.Bills.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.EventParticipant.News.ID.Equals(EventID) && c.CreatedDate.Year == Date.Year && c.CreatedDate.Month == Date.Month && c.CreatedDate.Day == Date.Day).Sum(b => b.Price);
                return SumPriceBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IPagedList<NumberOfGiftExchange>> StatisticsGiftExchange(int page, int limit, string SeachKey, string fromDate, string toDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);

                    var Model = (from a in DbContext.News
                                 where a.IsActive.Equals(SystemParam.ACTIVE)
                                 && a.Status.Equals(SystemParam.ACTIVE)
                                 && a.Type.Equals(SystemParam.TYPE_NEWS_EVENT)
                                 && (!string.IsNullOrEmpty(SeachKey) ? a.Title.Contains(SeachKey) : true)
                                 && (fd.HasValue ? a.CreatedDate >= fd : true)
                                 && (td.HasValue ? a.CreatedDate <= td : true)
                                 select new NumberOfGiftExchange
                                 {
                                     ID = a.ID,
                                     NameCampaign = a.Title,
                                     NumberCustomer = a.EventParticipants.GroupBy(c => c.Phone).Count(),//done
                                     GiftExchange = (from b in DbContext.EventParticipants
                                                     join c in DbContext.GiftEventParticipants on b.ID equals c.EventParticipantID
                                                     where b.NewsID == a.ID && b.IsActive.Equals(SystemParam.ACTIVE)
                                                     group c by c.EventParticipantID into c
                                                     select c).Count(),
                                     RemainingGift = a.GiftEvent.Where(e => e.NewsID.Equals(a.ID)).Sum(f => f.Quantity),//done
                                     NumberBill = a.EventParticipants.Sum(x => x.Bills.Count()),
                                 }).ToPagedList(page, limit);

                    return Model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IPagedList<NumberOfGiftExchangeDetail>> GetNumberOfGiftExchangeDetail(int page, int limit, int ID, string SearchKey, string fromDate, string toDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);

                    var Model = (from a in DbContext.EventParticipants
                                 where a.NewsID.Equals(ID)
                                 && a.IsActive.Equals(SystemParam.ACTIVE)
                                 && a.News.Type.Equals(SystemParam.TYPE_NEWS_EVENT)
                                 && (!string.IsNullOrEmpty(SearchKey) ? a.Customer.Name.Contains(SearchKey) || a.Customer.Phone.Contains(SearchKey) : true)
                                 && (fd.HasValue ? a.CreatedDate >= fd : true)
                                 && (td.HasValue ? a.CreatedDate <= td : true)
                                 orderby a.CreatedDate descending
                                 select new NumberOfGiftExchangeDetail
                                 {
                                     ID = a.ID,
                                     NameCus = a.Customer.Name,
                                     Phone = a.Customer.Phone,
                                     NumberGift = a.GiftEventParticipants.Count(),
                                     GiftVouchers = (from b in DbContext.GiftEventParticipants
                                                     where b.EventParticipantID.Equals(a.ID)
                                                     select new GiftVoucher
                                                     {
                                                         ID = b.ID,
                                                         Name = b.Gift.Name
                                                     }).ToList(),
                                     TotalAmount = a.Bills.Sum(c => c.Price),
                                     Date = a.CreatedDate.ToString("HH:mm dd/MM/yyyy")
                                 }).ToPagedList(page, limit);
                    return Model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<NumberOfGiftExchangeDetail>> GetALLNumberOfGiftExchangeDetail(int ID, string SearchKey, string fromDate, string toDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fd = Util.ConvertFromDate(fromDate);
                    var td = Util.ConvertToDate(toDate);

                    var Model = (from a in DbContext.EventParticipants
                                 where a.NewsID.Equals(ID)
                                 && a.IsActive.Equals(SystemParam.ACTIVE)
                                 && a.News.Type.Equals(SystemParam.TYPE_NEWS_EVENT)
                                 && (!string.IsNullOrEmpty(SearchKey) ? a.Customer.Name.Contains(SearchKey) || a.Customer.Phone.Contains(SearchKey) : true)
                                 && (fd.HasValue ? a.CreatedDate >= fd : true)
                                 && (td.HasValue ? a.CreatedDate <= td : true)
                                 orderby a.CreatedDate descending
                                 select new NumberOfGiftExchangeDetail
                                 {
                                     ID = a.ID,
                                     NameCus = a.Customer.Name,
                                     Phone = a.Customer.Phone,
                                     NumberGift = a.GiftEventParticipants.Count(),
                                     GiftVouchers = (from b in DbContext.GiftEventParticipants
                                                     where b.EventParticipantID.Equals(a.ID)
                                                     select new GiftVoucher
                                                     {
                                                         ID = b.ID,
                                                         Name = b.Gift.Name
                                                     }).ToList(),
                                     TotalAmount = a.Bills.Sum(c => c.Price),
                                     Date = a.CreatedDate.ToString("HH:mm dd/MM/yyyy")
                                 }).ToList();
                    return Model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IPagedList<GiftVoucher>> GetGiftVoucherDetail(int page, int limit, int ID, string SeachKey)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var Model = (from a in DbContext.GiftEvents
                                 where a.IsActive.Equals(SystemParam.ACTIVE)
                                 && a.NewsID.Equals(ID)
                                 && (!string.IsNullOrEmpty(SeachKey) ? a.Gift.Name.Contains(SeachKey) : true)
                                 select new GiftVoucher
                                 {
                                     ID = a.ID,
                                     Name = a.Gift.Name,
                                     Quantity = a.Quantity,
                                     QuantityExchanged = a.QuantityExchanged
                                 }).ToPagedList(page, limit);
                    return Model;


                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}